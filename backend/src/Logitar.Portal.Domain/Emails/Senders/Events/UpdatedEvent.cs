using Logitar.Portal.Core.Emails.Senders.Payloads;

namespace Logitar.Portal.Domain.Emails.Senders.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateSenderPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateSenderPayload Payload { get; private set; }
  }
}
