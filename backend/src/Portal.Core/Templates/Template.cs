﻿using Portal.Core.Realms;
using Portal.Core.Templates.Events;
using Portal.Core.Templates.Payloads;

namespace Portal.Core.Templates
{
  public class Template : Aggregate
  {
    public Template(CreateTemplatePayload payload, Guid userId, Realm? realm = null)
    {
      ApplyChange(new CreatedEvent(payload, userId));

      Realm = realm;
      RealmSid = realm?.Sid;
    }
    private Template()
    {
    }

    public Realm? Realm { get; private set; }
    public int? RealmSid { get; private set; }

    public string Key { get; private set; } = null!;
    public string KeyNormalized
    {
      get => Key.ToUpper();
      private set { /* EntityFrameworkCore only setter */ }
    }

    public string Contents { get; private set; } = null!;

    public string? DisplayName { get; private set; }
    public string? Description { get; private set; }

    public void Delete(Guid userId) => ApplyChange(new DeletedEvent(userId));
    public void Update(UpdateTemplatePayload payload, Guid userId) => ApplyChange(new UpdatedEvent(payload, userId));

    protected virtual void Apply(CreatedEvent @event)
    {
      Key = @event.Payload.Key;

      Apply(@event.Payload);
    }
    protected virtual void Apply(DeletedEvent @event)
    {
    }
    protected virtual void Apply(UpdatedEvent @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveTemplatePayload payload)
    {
      Contents = payload.Contents.Trim();

      DisplayName = payload.DisplayName?.CleanTrim();
      Description = payload.Description?.CleanTrim();
    }

    public override string ToString() => $"{DisplayName ?? Key} | {base.ToString()}";
  }
}