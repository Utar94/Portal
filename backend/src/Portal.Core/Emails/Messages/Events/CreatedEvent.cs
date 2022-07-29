﻿using Portal.Core.Emails.Senders;

namespace Portal.Core.Emails.Messages.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(string body, IEnumerable<Recipient> recipients,
      Guid? realmId, string? realmAlias, string? realmName,
      Guid senderId, bool senderIsDefault, ProviderType senderProvider, string senderAddress, string? senderDisplayName,
      Guid templateId, string templateKey, string templateSubject, string templateContentType, string? templateDisplayName,
      Dictionary<string, string?>? variables, Guid userId) : base(userId)
    {
      Body = body ?? throw new ArgumentNullException(nameof(body));
      RealmAlias = realmAlias;
      RealmId = realmId;
      RealmName = realmName;
      Recipients = recipients ?? throw new ArgumentNullException(nameof(recipients));
      SenderAddress = senderAddress ?? throw new ArgumentNullException(nameof(senderAddress));
      SenderDisplayName = senderDisplayName;
      SenderId = senderId;
      SenderIsDefault = senderIsDefault;
      SenderProvider = senderProvider;
      TemplateContentType = templateContentType ?? throw new ArgumentNullException(nameof(templateContentType));
      TemplateDisplayName = templateDisplayName;
      TemplateId = templateId;
      TemplateKey = templateKey ?? throw new ArgumentNullException(nameof(templateKey));
      TemplateSubject = templateSubject ?? throw new ArgumentNullException(nameof(templateSubject));
      Variables = variables;
    }

    public string Body { get; private set; }
    public string? RealmAlias { get; private set; }
    public Guid? RealmId { get; private set; }
    public string? RealmName { get; private set; }
    public IEnumerable<Recipient> Recipients { get; private set; }
    public string SenderAddress { get; private set; }
    public string? SenderDisplayName { get; private set; }
    public Guid SenderId { get; private set; }
    public bool SenderIsDefault { get; private set; }
    public ProviderType SenderProvider { get; private set; }
    public string TemplateContentType { get; private set; }
    public string? TemplateDisplayName { get; private set; }
    public string TemplateSubject { get; private set; }
    public Guid TemplateId { get; private set; }
    public string TemplateKey { get; private set; }
    public Dictionary<string, string?>? Variables { get; private set; }
  }
}
