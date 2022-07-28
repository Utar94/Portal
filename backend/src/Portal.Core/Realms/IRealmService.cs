﻿using Portal.Core.Realms.Models;
using Portal.Core.Realms.Payloads;

namespace Portal.Core.Realms
{
  public interface IRealmService
  {
    Task<RealmModel> CreateAsync(CreateRealmPayload payload, CancellationToken cancellationToken = default);
    Task<RealmModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RealmModel?> GetAsync(string id, CancellationToken cancellationToken = default);
    Task<ListModel<RealmModel>> GetAsync(string? search = null,
      RealmSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<RealmModel> UpdateAsync(Guid id, UpdateRealmPayload payload, CancellationToken cancellationToken = default);
  }
}
