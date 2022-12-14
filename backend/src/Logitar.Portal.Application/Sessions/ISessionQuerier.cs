using Logitar.Portal.Core.Sessions;
using Logitar.Portal.Domain.Sessions;

namespace Logitar.Portal.Application.Sessions
{
  public interface ISessionQuerier
  {
    Task<Session?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Session>> GetPagedAsync(bool? isActive = null, bool? isPersistent = null, string? realm = null, Guid? userId = null,
      SessionSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
