using Logitar.Portal.Core;
using Logitar.Portal.Domain.Users;
using System.Net;

namespace Logitar.Portal.Application.Users
{
  internal class UserNotDisabledException : ApiException
  {
    public UserNotDisabledException(User user)
      : base(HttpStatusCode.BadRequest, $"The user '{user}' is not disabled.")
    {
      User = user ?? throw new ArgumentNullException(nameof(user));
      Value = new { code = nameof(UserNotDisabledException).Remove(nameof(Exception)) };
    }

    public User User { get; }
  }
}
