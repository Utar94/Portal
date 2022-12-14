using Logitar.Portal.Core.Dictionaries;
using Logitar.Portal.Domain.Dictionaries;

namespace Logitar.Portal.Application.Dictionaries
{
  public interface IDictionaryQuerier
  {
    Task<Dictionary?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Dictionary>> GetPagedAsync(string? locale = null, string? realm = null,
      DictionarySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
