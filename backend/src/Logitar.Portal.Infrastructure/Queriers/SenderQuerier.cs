using Logitar.Portal.Application;
using Logitar.Portal.Application.Emails.Senders;
using Logitar.Portal.Core.Emails.Senders;
using Logitar.Portal.Domain.Emails.Senders;
using Logitar.Portal.Domain.Realms;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Portal.Infrastructure.Queriers
{
  internal class SenderQuerier : ISenderQuerier
  {
    private readonly DbSet<Sender> _senders;

    public SenderQuerier(PortalDbContext dbContext)
    {
      _senders = dbContext.Senders;
    }

    public async Task<Sender?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _senders.ApplyTracking(readOnly)
        .Include(x => x.Realm)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Sender?> GetDefaultAsync(Realm? realm, bool readOnly = false, CancellationToken cancellationToken = default)
    {
      IQueryable<Sender> query = _senders.ApplyTracking(readOnly).Include(x => x.Realm);

      return realm == null
        ? await query.SingleOrDefaultAsync(x => x.RealmSid == null && x.IsDefault, cancellationToken)
        : await query.SingleOrDefaultAsync(x => x.RealmSid == realm.Sid && x.IsDefault, cancellationToken);
    }

    public async Task<PagedList<Sender>> GetPagedAsync(ProviderType? provider, string? realm, string? search,
      SenderSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Sender> query = _senders.ApplyTracking(readOnly)
        .Include(x => x.Realm);

      if (realm == null)
      {
        query = query.Where(x => x.RealmSid == null);
      }
      else
      {
        query = Guid.TryParse(realm, out Guid realmId)
          ? query.Where(x => x.Realm != null && x.Realm.Id == realmId)
          : query.Where(x => x.Realm != null && x.Realm.AliasNormalized == realm.ToUpper());
      }

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.EmailAddress, pattern)
              || (x.DisplayName != null && EF.Functions.ILike(x.DisplayName, pattern)));
          }
        }
      }
      if (provider.HasValue)
      {
        query = query.Where(x => x.Provider == provider.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          SenderSort.DisplayName => desc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName),
          SenderSort.EmailAddress => desc ? query.OrderByDescending(x => x.EmailAddress) : query.OrderBy(x => x.EmailAddress),
          SenderSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The sender sort '{sort}' is not valid.", nameof(sort)),
        };
      }

      query = query.ApplyPaging(index, count);

      Sender[] senders = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Sender>(senders, total);
    }
  }
}
