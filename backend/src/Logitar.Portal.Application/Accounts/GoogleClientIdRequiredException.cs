using Logitar.Portal.Core;
using Logitar.Portal.Domain.Realms;
using System.Net;

namespace Logitar.Portal.Application.Accounts
{
  internal class GoogleClientIdRequiredException : ApiException
  {
    public GoogleClientIdRequiredException(Realm realm)
      : base(HttpStatusCode.BadRequest, $"The realm '{realm}' does not have a configured Google Client ID.")
    {
      Realm = realm ?? throw new ArgumentNullException(nameof(realm));
      Value = new { code = nameof(GoogleClientIdRequiredException).Remove(nameof(Exception)) };
    }

    public Realm Realm { get; }
  }
}
