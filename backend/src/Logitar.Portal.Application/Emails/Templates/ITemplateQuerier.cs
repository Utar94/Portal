using Logitar.Portal.Core.Emails.Templates;
using Logitar.Portal.Domain.Emails.Templates;
using Logitar.Portal.Domain.Realms;

namespace Logitar.Portal.Application.Emails.Templates
{
  public interface ITemplateQuerier
  {
    Task<Template?> GetAsync(string key, Realm? realm = null, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<Template?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<Template?> GetByKeyAsync(string key, Realm? realm = null, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Template>> GetPagedAsync(string? realm = null, string? search = null,
      TemplateSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
