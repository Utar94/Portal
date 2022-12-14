using FluentValidation;
using Logitar.Portal.Application.Dictionaries;
using Logitar.Portal.Application.Emails.Senders;
using Logitar.Portal.Application.Emails.Templates;
using Logitar.Portal.Application.Realms;
using Logitar.Portal.Application.Users;
using Logitar.Portal.Core;
using Logitar.Portal.Core.Emails.Messages;
using Logitar.Portal.Core.Emails.Messages.Models;
using Logitar.Portal.Core.Emails.Messages.Payloads;
using Logitar.Portal.Domain.Dictionaries;
using Logitar.Portal.Domain.Emails.Messages;
using Logitar.Portal.Domain.Emails.Senders;
using Logitar.Portal.Domain.Emails.Templates;
using Logitar.Portal.Domain.Realms;
using Logitar.Portal.Domain.Users;
using System.Globalization;

namespace Logitar.Portal.Application.Emails.Messages
{
  internal class MessageService : IMessageService
  {
    private readonly IDictionaryQuerier _dictionaryQuerier;
    private readonly IMessageHandlerFactory _handlerFactory;
    private readonly IMappingService _mappingService;
    private readonly IMessageQuerier _querier;
    private readonly IRealmQuerier _realmQuerier;
    private readonly IRepository<Message> _repository;
    private readonly IValidator<SendDemoMessagePayload> _sendDemoMessageValidator;
    private readonly IValidator<SendMessagePayload> _sendMessageValidator;
    private readonly ISenderQuerier _senderQuerier;
    private readonly ITemplateCompiler _templateCompiler;
    private readonly ITemplateQuerier _templateQuerier;
    private readonly IUserContext _userContext;
    private readonly IUserQuerier _userQuerier;

    public MessageService(
      IDictionaryQuerier dictionaryQuerier,
      IMessageHandlerFactory handlerFactory,
      IMappingService mapper,
      IMessageQuerier querier,
      IRealmQuerier realmQuerier,
      IRepository<Message> repository,
      IValidator<SendDemoMessagePayload> sendDemoMessageValidator,
      IValidator<SendMessagePayload> sendMessageValidator,
      ISenderQuerier senderQuerier,
      ITemplateCompiler templateCompiler,
      ITemplateQuerier templateQuerier,
      IUserContext userContext,
      IUserQuerier userQuerier
    )
    {
      _dictionaryQuerier = dictionaryQuerier;
      _handlerFactory = handlerFactory;
      _mappingService = mapper;
      _querier = querier;
      _realmQuerier = realmQuerier;
      _repository = repository;
      _sendDemoMessageValidator = sendDemoMessageValidator;
      _sendMessageValidator = sendMessageValidator;
      _senderQuerier = senderQuerier;
      _templateCompiler = templateCompiler;
      _templateQuerier = templateQuerier;
      _userContext = userContext;
      _userQuerier = userQuerier;
    }

    public async Task<MessageModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Message? message = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (message == null)
      {
        return null;
      }

      return await _mappingService.MapAsync<MessageModel>(message, cancellationToken);
    }

    public async Task<ListModel<MessageModel>> GetAsync(bool? hasErrors, bool? isDemo, string? realm, string? search, bool? succeeded, string? template,
      MessageSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Message> messages = await _querier.GetPagedAsync(hasErrors, isDemo, realm, search, succeeded, template,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return await _mappingService.MapAsync<Message, MessageModel>(messages, cancellationToken);
    }

    public async Task<SentMessagesModel> SendAsync(SendMessagePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      _sendMessageValidator.ValidateAndThrow(payload);

      Realm? realm = payload.Realm == null ? null : await ResolveRealmAsync(payload.Realm, nameof(payload.Realm), cancellationToken);

      Recipients allRecipients = await ResolveRecipientsAsync(payload.Recipients, realm, nameof(payload.Recipients), cancellationToken);
      Sender sender = await ResolveSenderAsync(payload.SenderId, realm, nameof(payload.SenderId), cancellationToken);
      Template template = await ResolveTemplateAsync(payload.Template, realm, nameof(payload.Template), cancellationToken);

      Dictionary<CultureInfo, Dictionary> allDictionaries = (await _dictionaryQuerier
        .GetPagedAsync(realm: realm?.Id.ToString(), readOnly: true, cancellationToken: cancellationToken)
      ).ToDictionary(x => x.Culture, x => x);
      Dictionary? defaultDictionary = null;
      if (realm?.DefaultCulture != null)
      {
        allDictionaries.TryGetValue(realm.DefaultCulture, out defaultDictionary);
      }

      Dictionary<string, string?>? variables = payload.Variables == null ? null : GetVariables(payload.Variables);

      IMessageHandler handler = _handlerFactory.GetHandler(sender);

      Dictionaries? dictionaries = payload.IgnoreUserLocale
        ? GetDictionaries(payload.Locale, defaultDictionary, allDictionaries)
        : null;

      var messages = new List<Message>(capacity: allRecipients.To.Count);
      foreach (Recipient recipient in allRecipients.To)
      {
        Dictionaries? userDictionaries = (!payload.IgnoreUserLocale && recipient.UserLocale != null)
          ? GetDictionaries(recipient.UserLocale, defaultDictionary, allDictionaries)
          : null;

        string subject = (userDictionaries ?? dictionaries)?.GetEntry(template.Subject) ?? template.Subject;
        string body = _templateCompiler.Compile(template, userDictionaries ?? dictionaries, recipient.User, variables);

        IEnumerable<Recipient> recipients = new[] { recipient }
          .Concat(allRecipients.CC)
          .Concat(allRecipients.Bcc);

        Message message = await CreateAndSendAsync(body,
          handler,
          ignoreUserLocale: payload.IgnoreUserLocale,
          locale: payload.IgnoreUserLocale ? payload.Locale : (recipient.UserLocale ?? payload.Locale),
          realm,
          recipients,
          sender,
          subject,
          template,
          variables,
          isDemo: false,
          cancellationToken);

        messages.Add(message);
      }

      await _repository.SaveAsync(messages, cancellationToken);

      return new SentMessagesModel
      {
        Error = messages.Where(x => x.HasErrors).Select(x => x.Id),
        Success = messages.Where(x => x.Succeeded).Select(x => x.Id),
        Unsent = messages.Where(x => !x.HasErrors && !x.Succeeded).Select(x => x.Id)
      };
    }

    public async Task<MessageModel> SendDemoAsync(SendDemoMessagePayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      _sendDemoMessageValidator.ValidateAndThrow(payload);

      User user = await _userQuerier.GetAsync(_userContext.Id, readOnly: true, cancellationToken)
        ?? throw new InvalidOperationException($"The user 'Id={_userContext.Id}' could not be found.");
      if (user.Email == null)
      {
        throw new UserEmailRequiredException(user.Id);
      }
      var recipients = new Recipient[] { new(user) };

      Template template = await _templateQuerier.GetAsync(payload.TemplateId, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Template>(payload.TemplateId, nameof(payload.TemplateId));
      Realm? realm = template.Realm;

      Sender sender = await _senderQuerier.GetDefaultAsync(realm, readOnly: true, cancellationToken)
        ?? throw new DefaultSenderRequiredException(realm);

      Dictionary<CultureInfo, Dictionary> allDictionaries = (await _dictionaryQuerier
        .GetPagedAsync(realm: realm?.Id.ToString(), readOnly: true, cancellationToken: cancellationToken)
      ).ToDictionary(x => x.Culture, x => x);
      Dictionary? defaultDictionary = null;
      if (realm?.DefaultCulture != null)
      {
        allDictionaries.TryGetValue(realm.DefaultCulture, out defaultDictionary);
      }
      Dictionaries dictionaries = GetDictionaries(payload.Locale ?? user.Locale, defaultDictionary, allDictionaries);

      Dictionary<string, string?>? variables = payload.Variables == null ? null : GetVariables(payload.Variables);

      IMessageHandler handler = _handlerFactory.GetHandler(sender);

      string subject = dictionaries.GetEntry(template.Subject) ?? template.Subject;
      string body = _templateCompiler.Compile(template, dictionaries, user, variables);

      Message message = await CreateAndSendAsync(body,
        handler,
        ignoreUserLocale: payload.Locale != null,
        payload.Locale ?? user.Locale,
        realm,
        recipients,
        sender,
        subject,
        template,
        variables,
        isDemo: true,
        cancellationToken);

      await _repository.SaveAsync(message, cancellationToken);

      return await _mappingService.MapAsync<MessageModel>(message, cancellationToken);
    }

    private async Task<Message> CreateAndSendAsync(string body,
      IMessageHandler handler,
      bool ignoreUserLocale,
      string? locale,
      Realm? realm,
      IEnumerable<Recipient> recipients,
      Sender sender,
      string subject,
      Template template,
      Dictionary<string, string?>? variables,
      bool isDemo,
      CancellationToken cancellationToken
    )
    {
      var message = new Message(subject, body, recipients, sender, template, _userContext.Actor.Id, realm, ignoreUserLocale, locale, variables, isDemo);

      try
      {
        SendMessageResult result = await handler.SendAsync(message, cancellationToken);
        message.Succeed(result, _userContext.Actor.Id);
      }
      catch (ErrorException exception)
      {
        message.Fail(exception.Error, _userContext.Actor.Id);
      }
      catch (Exception exception)
      {
        message.Fail(new Error(exception), _userContext.Actor.Id);
      }

      return message;
    }

    private static Dictionaries GetDictionaries(string? locale, Dictionary? defaultDictionary, Dictionary<CultureInfo, Dictionary> dictionaries)
    {
      Dictionary? preferredDictionary = null;
      Dictionary? fallbackDictionary = null;

      if (locale != null)
      {
        var preferred = CultureInfo.GetCultureInfo(locale);
        dictionaries.TryGetValue(preferred, out preferredDictionary);

        if (preferred.Parent != null)
        {
          dictionaries.TryGetValue(preferred.Parent, out fallbackDictionary);
        }
      }

      return new Dictionaries(defaultDictionary, fallbackDictionary, preferredDictionary);
    }

    private static Dictionary<string, string?>? GetVariables(IEnumerable<VariablePayload> payloads)
    {
      ArgumentNullException.ThrowIfNull(payloads);

      Dictionary<string, string?>? variables = payloads
        ?.GroupBy(x => x.Key)
        .ToDictionary(x => x.Key, x => x.FirstOrDefault(y => y.Value != null)?.Value);

      return variables;
    }

    private async Task<Realm?> ResolveRealmAsync(string id, string paramName, CancellationToken cancellationToken)
    {
      Realm realm = await _realmQuerier.GetAsync(id, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Realm>(id, paramName);

      return realm;
    }

    private async Task<Recipients> ResolveRecipientsAsync(IEnumerable<RecipientPayload> payloads, Realm? realm, string paramName, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payloads);

      var userIds = new List<Guid>(capacity: payloads.Count());
      var usernames = new List<string>(userIds.Capacity);
      foreach (RecipientPayload payload in payloads)
      {
        if (payload.User != null)
        {
          if (Guid.TryParse(payload.User, out Guid userId))
          {
            userIds.Add(userId);
          }
          else
          {
            usernames.Add(payload.User);
          }
        }
      }

      Dictionary<Guid, User> usersById = (await _userQuerier.GetAsync(userIds, readOnly: true, cancellationToken))
        .ToDictionary(x => x.Id, x => x);
      Dictionary<string, User> usersByUsername = (await _userQuerier.GetAsync(usernames, realm, readOnly: true, cancellationToken))
        .ToDictionary(x => x.UsernameNormalized, x => x);

      var missingUsers = new List<string>(userIds.Capacity);
      var notInRealm = new List<Guid>(userIds.Capacity);
      var missingEmails = new List<Guid>(userIds.Capacity);
      var to = new List<Recipient>(userIds.Capacity);
      var cc = new List<Recipient>(userIds.Capacity);
      var bcc = new List<Recipient>(userIds.Capacity);
      foreach (RecipientPayload recipientPayload in payloads)
      {
        User? user = null;
        if (recipientPayload.User != null)
        {
          if (Guid.TryParse(recipientPayload.User, out Guid userId))
          {
            usersById.TryGetValue(userId, out user);
          }
          else
          {
            usersByUsername.TryGetValue(recipientPayload.User.ToUpper(), out user);
          }

          if (user == null)
          {
            missingUsers.Add(recipientPayload.User);
            continue;
          }
          else if (realm?.Sid != user.RealmSid)
          {
            notInRealm.Add(user.Id);
            continue;
          }
          else if (user.Email == null)
          {
            missingEmails.Add(user.Id);
            continue;
          }
        }

        Recipient recipient = user == null
          ? new(recipientPayload.Address!, recipientPayload.DisplayName, recipientPayload.Type)
          : new(user, recipientPayload.Type);

        switch (recipientPayload.Type)
        {
          case RecipientType.Bcc:
            bcc.Add(recipient);
            break;
          case RecipientType.CC:
            cc.Add(recipient);
            break;
          case RecipientType.To:
            to.Add(recipient);
            break;
          default:
            throw new NotSupportedException($"The recipient type '{recipientPayload.Type}' is not supported.");
        }
      }

      if (missingUsers.Any())
      {
        throw new UsersNotFoundException(missingUsers, paramName);
      }

      if (notInRealm.Any())
      {
        throw new UsersNotInRealmException(notInRealm, realm, paramName);
      }

      if (missingEmails.Any())
      {
        throw new UsersEmailRequiredException(missingEmails, paramName);
      }

      return new Recipients(to, cc, bcc);
    }

    private async Task<Sender> ResolveSenderAsync(Guid? id, Realm? realm, string paramName, CancellationToken cancellationToken)
    {
      Sender sender;
      if (id.HasValue)
      {
        sender = await _senderQuerier.GetAsync(id.Value, readOnly: true, cancellationToken)
          ?? throw new EntityNotFoundException<Sender>(id.Value, paramName);
      }
      else
      {
        sender = await _senderQuerier.GetDefaultAsync(realm, readOnly: true, cancellationToken)
          ?? throw new DefaultSenderRequiredException(realm);
      }

      if (realm?.Sid != sender.RealmSid)
      {
        throw new SenderNotInRealmException(sender, realm, paramName);
      }

      return sender;
    }

    private async Task<Template> ResolveTemplateAsync(string id, Realm? realm, string paramName, CancellationToken cancellationToken)
    {
      Template template = await _templateQuerier.GetAsync(id, realm, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Template>(id, paramName);

      if (realm?.Sid != template.RealmSid)
      {
        throw new TemplateNotInRealmException(template, realm, paramName);
      }

      return template;
    }
  }
}
