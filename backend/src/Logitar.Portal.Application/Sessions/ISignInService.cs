using Logitar.Portal.Core.Sessions.Models;
using Logitar.Portal.Domain.Sessions;
using Logitar.Portal.Domain.Users;

namespace Logitar.Portal.Application.Sessions
{
  public interface ISignInService
  {
    Task<SessionModel> RenewAsync(Session session, string? ipAddress = null, string? additionalInformation = null, CancellationToken cancellationToken = default);
    Task<SessionModel> SignInAsync(User user, bool remember = false, string? ipAddress = null, string? additionalInformation = null, CancellationToken cancellationToken = default);
  }
}
