﻿using Portal.Core.Emails.Messages.Models;
using Portal.Core.Emails.Messages.Payloads;

namespace Portal.Core.Emails.Messages
{
  public interface IMessageService
  {
    Task<MessageModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<MessageSummary>> GetAsync(bool? hasErrors = null, Guid? realmId = null, string? search = null, bool? succeeded = null, Guid? templateId = null,
      MessageSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<SentMessagesModel> SendAsync(SendMessagePayload payload, CancellationToken cancellationToken = default);
    Task<MessageModel> SendDemoAsync(SendDemoMessagePayload payload, CancellationToken cancellationToken = default);
  }
}
