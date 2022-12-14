namespace Logitar.Portal.Domain.Emails.Senders.Events
{
  public class SetDefaultEvent : UpdatedEventBase
  {
    public SetDefaultEvent(Guid userId, bool isDefault = true) : base(userId)
    {
      IsDefault = isDefault;
    }

    public bool IsDefault { get; private set; }
  }
}
