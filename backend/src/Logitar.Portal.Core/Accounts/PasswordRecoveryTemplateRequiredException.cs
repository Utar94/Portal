﻿using Logitar.Portal.Core.Realms;
using System.Net;

namespace Logitar.Portal.Core.Accounts
{
  internal class PasswordRecoveryTemplateRequiredException : ApiException
  {
    public PasswordRecoveryTemplateRequiredException(Realm realm)
      : base(HttpStatusCode.BadRequest, $"The realm '{realm}' does not have a configured password recovery.")
    {
      Realm = realm ?? throw new ArgumentNullException(nameof(realm));
      Value = new { code = nameof(PasswordRecoveryTemplateRequiredException).Remove(nameof(Exception)) };
    }

    public Realm Realm { get; }
  }
}
