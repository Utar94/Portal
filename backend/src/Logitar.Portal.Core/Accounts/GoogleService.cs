﻿using FluentValidation;
using Google.Apis.Auth;
using Logitar.Portal.Core.Accounts.Payloads;
using Logitar.Portal.Core.Realms;
using Logitar.Portal.Core.Sessions;
using Logitar.Portal.Core.Sessions.Models;
using Logitar.Portal.Core.Users;
using Logitar.Portal.Core.Users.Payloads;

namespace Logitar.Portal.Core.Accounts
{
  internal class GoogleService : IGoogleService
  {
    private readonly IRealmQuerier _realmQuerier;
    private readonly ISessionService _sessionService;
    private readonly IUserContext _userContext;
    private readonly IUserQuerier _userQuerier;
    private readonly IRepository<User> _userRepository;
    private readonly IValidator<User> _userValidator;

    public GoogleService(
      IRealmQuerier realmQuerier,
      ISessionService sessionService,
      IUserContext userContext,
      IUserQuerier userQuerier,
      IRepository<User> userRepository,
      IValidator<User> userValidator
    )
    {
      _realmQuerier = realmQuerier;
      _sessionService = sessionService;
      _userContext = userContext;
      _userQuerier = userQuerier;
      _userRepository = userRepository;
      _userValidator = userValidator;
    }

    public async Task<SessionModel> AuthenticateAsync(string realmId, AuthenticateWithGooglePayload payload, string? ipAddress, string? additionalInformation, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(realmId);
      ArgumentNullException.ThrowIfNull(payload);

      Realm realm = await _realmQuerier.GetAsync(realmId, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Realm>(realmId);

      if (realm.GoogleClientId == null)
      {
        throw new GoogleClientIdRequiredException(realm);
      }

      var validationSettings = new GoogleJsonWebSignature.ValidationSettings
      {
        Audience = new string[] { realm.GoogleClientId }
      };
      GoogleJsonWebSignature.Payload googlePayload = await GoogleJsonWebSignature.ValidateAsync(payload.Credential, validationSettings);

      User? user = await _userQuerier.GetByExternalProviderAsync(realm, ExternalProviders.Google, googlePayload.Subject, readOnly: false, cancellationToken);
      if (user == null)
      {
        user = await _userQuerier.GetAsync(googlePayload.Email, realm, readOnly: false, cancellationToken);
        if (user == null)
        {
          if (realm.RequireUniqueEmail)
          {
            user = (await _userQuerier.GetByEmailAsync(googlePayload.Email, realm, readOnly: false, cancellationToken))
              .SingleOrDefault();
          }

          if (user == null)
          {
            var userPayload = new CreateUserSecurePayload
            {
              ConfirmEmail = googlePayload.EmailVerified,
              Email = googlePayload.Email,
              FirstName = googlePayload.GivenName,
              LastName = googlePayload.FamilyName,
              Locale = payload.IgnoreProviderLocale ? payload.Locale : (googlePayload.Locale ?? payload.Locale),
              Picture = googlePayload.Picture,
              Realm = realmId,
              Username = googlePayload.Email
            };

            user = new User(userPayload, _userContext.ActorId, realm);
            if (userPayload.ConfirmEmail)
            {
              user.ConfirmEmail(_userContext.ActorId);
            }

            var context = ValidationContext<User>.CreateWithOptions(user, options => options.ThrowOnFailures());
            context.SetAllowedUsernameCharacters(realm.AllowedUsernameCharacters);
            _userValidator.Validate(context);

            await _userRepository.SaveAsync(user, cancellationToken);
          }
        }

        user.AddExternalProvider(ExternalProviders.Google, googlePayload.Subject, _userContext.ActorId, ExternalProviders.Google);

        await _userRepository.SaveAsync(user, cancellationToken);
      }

      return await _sessionService.SignInAsync(user, remember: true, ipAddress, additionalInformation, cancellationToken);
    }
  }
}
