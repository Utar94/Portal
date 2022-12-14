using Logitar.Portal.Core;
using Logitar.Portal.Core.Emails.Senders;
using Logitar.Portal.Domain.Emails.Messages.Events;
using Logitar.Portal.Domain.Emails.Senders;
using Logitar.Portal.Domain.Emails.Templates;
using Logitar.Portal.Domain.Realms;
using System.Text.Json;

namespace Logitar.Portal.Domain.Emails.Messages
{
  public class Message : Aggregate
  {
    public Message(string subject, string body, IEnumerable<Recipient> recipients, Sender sender, Template template,
      Guid userId, Realm? realm = null, bool ignoreUserLocale = false, string? locale = null,
      Dictionary<string, string?>? variables = null, bool isDemo = false)
    {
      ArgumentNullException.ThrowIfNull(sender);
      ArgumentNullException.ThrowIfNull(template);

      ApplyChange(new CreatedEvent(subject, body, recipients,
        realm?.Id, realm?.AliasNormalized, realm?.Name,
        sender.Id, sender.IsDefault, sender.Provider, sender.EmailAddress, sender.DisplayName,
        template.Id, template.KeyNormalized, template.ContentType, template.DisplayName,
        ignoreUserLocale, locale, variables, isDemo, userId));
    }
    private Message()
    {
    }

    public bool IsDemo { get; private set; }

    public string Subject { get; private set; } = null!;
    public string Body { get; private set; } = null!;

    public IEnumerable<Recipient> Recipients { get; private set; } = Enumerable.Empty<Recipient>();
    public string RecipientsSerialized
    {
      get => JsonSerializer.Serialize(Recipients);
      private set => Recipients = JsonSerializer.Deserialize<IEnumerable<Recipient>>(value) ?? Enumerable.Empty<Recipient>();
    }

    public Guid SenderId { get; private set; }
    public bool SenderIsDefault { get; private set; }
    public ProviderType SenderProvider { get; private set; }
    public string SenderAddress { get; private set; } = null!;
    public string? SenderDisplayName { get; private set; }

    public Guid? RealmId { get; private set; }
    public string? RealmAlias { get; private set; }
    public string? RealmName { get; private set; }

    public Guid TemplateId { get; private set; }
    public string TemplateKey { get; private set; } = null!;
    public string TemplateContentType { get; private set; } = null!;
    public string? TemplateDisplayName { get; private set; }

    public bool IgnoreUserLocale { get; private set; }
    public string? Locale { get; private set; }

    public Dictionary<string, string?>? Variables { get; private set; }
    public string? VariablesSerialized
    {
      get => Variables?.Any() == true ? JsonSerializer.Serialize(Variables) : null;
      private set => Variables = value == null ? null : JsonSerializer.Deserialize<Dictionary<string, string?>>(value);
    }

    public IEnumerable<Error> Errors { get; private set; } = Enumerable.Empty<Error>();
    public string? ErrorsSerialized
    {
      get => Errors.Any() ? JsonSerializer.Serialize(Errors) : null;
      private set => Errors = (value == null ? null : JsonSerializer.Deserialize<IEnumerable<Error>>(value)) ?? Enumerable.Empty<Error>();
    }
    public bool HasErrors
    {
      get => Errors.Any();
      private set { /* EntityFrameworkCore only setter */ }
    }

    public SendMessageResult? Result { get; private set; }
    public string? ResultSerialized
    {
      get => Result?.Any() == true ? JsonSerializer.Serialize(Result) : null;
      private set => Result = value == null ? null : JsonSerializer.Deserialize<SendMessageResult>(value);
    }
    public bool Succeeded
    {
      get => !HasErrors && Result != null;
      private set { /* EntityFrameworkCore only setter */ }
    }

    public void Fail(Error error, Guid userId)
    {
      ApplyChange(new FailedEvent(new[] { error }, userId));
    }
    public void Succeed(SendMessageResult result, Guid userId)
    {
      ApplyChange(new SucceededEvent(result, userId));
    }

    protected virtual void Apply(CreatedEvent @event)
    {
      IsDemo = @event.IsDemo;

      Subject = @event.Subject;
      Body = @event.Body.Trim();

      Recipients = @event.Recipients;

      SenderId = @event.SenderId;
      SenderIsDefault = @event.SenderIsDefault;
      SenderProvider = @event.SenderProvider;
      SenderAddress = @event.SenderAddress;
      SenderDisplayName = @event.SenderDisplayName;

      RealmId = @event.RealmId;
      RealmAlias = @event.RealmAlias;
      RealmName = @event.RealmName;

      TemplateId = @event.TemplateId;
      TemplateKey = @event.TemplateKey;
      TemplateContentType = @event.TemplateContentType;
      TemplateDisplayName = @event.TemplateDisplayName;

      IgnoreUserLocale = @event.IgnoreUserLocale;
      Locale = @event.Locale;

      Variables = @event.Variables;
    }
    protected virtual void Apply(FailedEvent @event)
    {
      Errors = @event.Errors;
    }
    protected virtual void Apply(SucceededEvent @event)
    {
      Result = @event.Result;
    }

    public override string ToString() => $"{Subject} | {base.ToString()}";
  }
}
