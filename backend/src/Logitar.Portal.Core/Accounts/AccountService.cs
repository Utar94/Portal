﻿using AutoMapper;
using Logitar.Portal.Core.Accounts.Payloads;
using Logitar.Portal.Core.Emails.Messages;
using Logitar.Portal.Core.Emails.Messages.Models;
using Logitar.Portal.Core.Emails.Messages.Payloads;
using Logitar.Portal.Core.Realms;
using Logitar.Portal.Core.Sessions;
using Logitar.Portal.Core.Sessions.Models;
using Logitar.Portal.Core.Tokens;
using Logitar.Portal.Core.Tokens.Models;
using Logitar.Portal.Core.Tokens.Payloads;
using Logitar.Portal.Core.Users;
using Logitar.Portal.Core.Users.Models;
using Logitar.Portal.Core.Users.Payloads;
using System.Text;

namespace Logitar.Portal.Core.Accounts
{
  internal class AccountService : IAccountService
  {
    private const int PasswordResetLifetime = 7 * 24 * 60 * 60; // 7 days
    private const string PasswordResetPurpose = "reset_password";

    private const int SessionKeyLength = 32;

    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly IPasswordService _passwordService;
    private readonly IRealmQuerier _realmQuerier;
    private readonly ISessionQuerier _sessionQuerier;
    private readonly IRepository<Session> _sessionRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserContext _userContext;
    private readonly IUserQuerier _userQuerier;
    private readonly IRepository<User> _userRepository;
    private readonly IUserService _userService;

    public AccountService(
      IMapper mapper,
      IMessageService messageService,
      IPasswordService passwordService,
      IRealmQuerier realmQuerier,
      ISessionQuerier sessionQuerier,
      IRepository<Session> sessionRepository,
      ITokenService tokenService,
      IUserContext userContext,
      IUserQuerier userQuerier,
      IRepository<User> userRepository,
      IUserService userService
    )
    {
      _mapper = mapper;
      _messageService = messageService;
      _passwordService = passwordService;
      _realmQuerier = realmQuerier;
      _sessionQuerier = sessionQuerier;
      _sessionRepository = sessionRepository;
      _tokenService = tokenService;
      _userContext = userContext;
      _userQuerier = userQuerier;
      _userRepository = userRepository;
      _userService = userService;
    }

    public async Task<UserModel> ChangePasswordAsync(ChangePasswordPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      User user = await _userQuerier.GetAsync(_userContext.Id, readOnly: false, cancellationToken)
        ?? throw new InvalidOperationException($"The user 'Id={_userContext.Id}' could not be found.");

      _passwordService.ValidateAndThrow(payload.Password, user.Realm);

      if (!_passwordService.IsMatch(user, payload.Current))
      {
        throw new InvalidCredentialsException();
      }

      string passwordHash = _passwordService.Hash(payload.Password);
      user.ChangePassword(passwordHash);
      await _userRepository.SaveAsync(user, cancellationToken);

      return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> GetProfileAsync(CancellationToken cancellationToken)
    {
      return await _userService.GetAsync(_userContext.Id, cancellationToken)
        ?? throw new InvalidOperationException($"The user 'Id={_userContext.Id}' could not be found.");
    }

    public async Task RecoverPasswordAsync(RecoverPasswordPayload payload, string realmId, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Realm realm = await _realmQuerier.GetAsync(realmId, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Realm>(realmId);

      User user = await _userQuerier.GetAsync(payload.Username, realm, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<User>(payload.Username, nameof(payload.Username));

      if (realm.PasswordRecoveryTemplate == null)
      {
        throw new PasswordRecoveryTemplateRequiredException(realm);
      }

      var createTokenPayload = new CreateTokenPayload
      {
        Lifetime = PasswordResetLifetime,
        Purpose = PasswordResetPurpose,
        Realm = realm.Id.ToString(),
        Subject = user.Id.ToString()
      };
      TokenModel token = await _tokenService.CreateAsync(createTokenPayload, cancellationToken);

      var sendMessagePayload = new SendMessagePayload
      {
        Realm = realm.Id.ToString(),
        Template = realm.PasswordRecoveryTemplate.Id.ToString(),
        SenderId = realm.PasswordRecoverySender?.Id,
        Recipients = new[] { new RecipientPayload { User = user.Id.ToString() } },
        Variables = new[] { new VariablePayload { Key = "Token", Value = token.Token } }
      };
      SentMessagesModel sentMessages = await _messageService.SendAsync(sendMessagePayload, cancellationToken);

      if (sentMessages.Error.Any() || sentMessages.Unsent.Any())
      {
        var message = new StringBuilder();

        message.AppendLine("The password recovery message sending failed.");

        if (sentMessages.Error.Any())
        {
          message.AppendLine($"Error IDs: {string.Join(", ", sentMessages.Error)}");
        }
        if (sentMessages.Unsent.Any())
        {
          message.AppendLine($"Unsent IDs: {string.Join(", ", sentMessages.Unsent)}");
        }

        throw new InvalidOperationException(message.ToString());
      }
      else if (sentMessages.Success.Count() != 1)
      {
        var message = new StringBuilder();

        if (!sentMessages.Success.Any())
        {
          message.AppendLine("No password recovery message was sent.");
          message.AppendLine($"User ID: {user.Id}");
        }
        else if (sentMessages.Success.Count() > 1)
        {
          message.AppendLine("No password recovery message was sent.");
          message.AppendLine($"User ID: {user.Id}");
          message.AppendLine($"Message IDs: {string.Join(", ", sentMessages.Success)}");
        }

        throw new InvalidOperationException(message.ToString());
      }
    }

    public async Task<SessionModel> RenewSessionAsync(RenewSessionPayload payload, string? realmId, string? ipAddress, string? additionalInformation, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Realm? realm = null;
      if (realmId != null)
      {
        realm = await _realmQuerier.GetAsync(realmId, readOnly: true, cancellationToken)
          ?? throw new EntityNotFoundException<Realm>(realmId);
      }

      if (!SecureToken.TryParse(payload.RenewToken, out SecureToken? secureToken) || secureToken == null)
      {
        throw new InvalidCredentialsException();
      }

      Session? session = await _sessionQuerier.GetAsync(secureToken.Id, readOnly: false, cancellationToken);
      if (session?.KeyHash == null || !session.IsActive || !_passwordService.IsMatch(session.KeyHash, secureToken.Key) || session.User?.RealmSid != realm?.Sid)
      {
        throw new InvalidCredentialsException();
      }

      string keyHash = _passwordService.GenerateAndHash(SessionKeyLength, out byte[] keyBytes);
      session.Update(keyHash, ipAddress, additionalInformation);
      await _sessionRepository.SaveAsync(session, cancellationToken);

      var model = _mapper.Map<SessionModel>(session);
      model.RenewToken = new SecureToken(model.Id, keyBytes).ToString();

      return model;
    }

    public async Task ResetPasswordAsync(ResetPasswordPayload payload, string realmId, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Realm realm = await _realmQuerier.GetAsync(realmId, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Realm>(realmId);

      _passwordService.ValidateAndThrow(payload.Password, realm);

      var validateTokenPayload = new ValidateTokenPayload
      {
        Token = payload.Token,
        Purpose = PasswordResetPurpose,
        Realm = realm.Id.ToString()
      };
      ValidatedTokenModel token = await _tokenService.ValidateAsync(validateTokenPayload, consume: true, cancellationToken);
      Guid userId = Guid.Parse(token.Subject!);

      User user = await _userQuerier.GetAsync(userId, readOnly: false, cancellationToken)
        ?? throw new InvalidOperationException($"The user 'Id={userId}' could not be found.");

      string passwordHash = _passwordService.Hash(payload.Password);
      user.ChangePassword(passwordHash);
      await _userRepository.SaveAsync(user, cancellationToken);
    }

    public async Task<UserModel> SaveProfileAsync(UpdateUserPayload payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      return await _userService.UpdateAsync(_userContext.Id, payload, cancellationToken);
    }

    public async Task<SessionModel> SignInAsync(SignInPayload payload, string? realmId, string? ipAddress, string? additionalInformation, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      Realm? realm = null;
      if (realmId != null)
      {
        realm = await _realmQuerier.GetAsync(realmId, readOnly: true, cancellationToken)
          ?? throw new EntityNotFoundException<Realm>(realmId);
      }

      User? user = await _userQuerier.GetAsync(payload.Username, realm, readOnly: false, cancellationToken);
      if (user == null || !_passwordService.IsMatch(user, payload.Password))
      {
        throw new InvalidCredentialsException();
      }
      else if (realm?.RequireConfirmedAccount == true && !user.IsAccountConfirmed)
      {
        throw new AccountNotConfirmedException(user.Id);
      }
      else if (user.IsDisabled)
      {
        throw new AccountIsDisabledException(user.Id);
      }

      byte[]? keyBytes = null;
      string? keyHash = null;
      if (payload.Remember)
      {
        keyHash = _passwordService.GenerateAndHash(SessionKeyLength, out keyBytes);
      }

      var session = new Session(user, keyHash, ipAddress, additionalInformation);
      await _sessionRepository.SaveAsync(session, cancellationToken);

      user.SignIn(session.CreatedAt);
      await _userRepository.SaveAsync(user, cancellationToken);

      var model = _mapper.Map<SessionModel>(session);
      model.RenewToken = keyBytes == null ? null : new SecureToken(model.Id, keyBytes).ToString();

      return model;
    }

    public async Task SignOutAsync(CancellationToken cancellationToken)
    {
      Session session = await _sessionQuerier.GetAsync(_userContext.SessionId, readOnly: false, cancellationToken)
        ?? throw new InvalidOperationException($"The session 'Id={_userContext.SessionId}' could not be found.");

      session.SignOut();
      await _sessionRepository.SaveAsync(session, cancellationToken);
    }
  }
}