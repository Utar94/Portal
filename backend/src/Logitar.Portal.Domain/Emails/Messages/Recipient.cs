using Logitar.Portal.Core.Emails.Messages;
using Logitar.Portal.Domain.Users;
using System.Text.Json.Serialization;

namespace Logitar.Portal.Domain.Emails.Messages
{
  public class Recipient
  {
    public Recipient(User user, RecipientType type = RecipientType.To)
      : this(type, user?.Email ?? string.Empty, user?.FullName, user?.Id, user?.Locale, user?.Username)
    {
      User = user ?? throw new ArgumentNullException(nameof(user));
    }
    public Recipient(string address, string? displayName, RecipientType type = RecipientType.To)
      : this(type, address, displayName, userId: null, userLocale: null, username: null)
    {
      Address = address ?? throw new ArgumentNullException(nameof(address));
      DisplayName = displayName;

      Type = type;
    }

    /// <summary>
    /// Public constructor for deserialization
    /// </summary>
    /// <param name="type"></param>
    /// <param name="address"></param>
    /// <param name="displayName"></param>
    /// <param name="userId"></param>
    /// <param name="username"></param>
    /// <exception cref="ArgumentNullException"></exception>
    [JsonConstructor]
    public Recipient(RecipientType type, string address, string? displayName, Guid? userId, string? userLocale, string? username)
    {
      Type = type;

      Address = address ?? throw new ArgumentNullException(nameof(address));
      DisplayName = displayName;

      UserId = userId;
      UserLocale = userLocale;
      Username = username;
    }

    public RecipientType Type { get; private set; }

    public string Address { get; private set; }
    public string? DisplayName { get; private set; }

    public Guid? UserId { get; private set; }
    public string? UserLocale { get; private set; }
    public string? Username { get; private set; }

    [JsonIgnore]
    public User? User { get; private set; }

    public override string ToString() => DisplayName == null
      ? Address
      : $"{DisplayName} <{Address}>";
  }
}
