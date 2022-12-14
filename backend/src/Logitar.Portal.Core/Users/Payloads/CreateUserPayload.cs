namespace Logitar.Portal.Core.Users.Payloads
{
  public class CreateUserPayload : SaveUserPayload
  {
    public string? Realm { get; set; }

    public string Username { get; set; } = null!;

    public bool ConfirmEmail { get; set; }
    public bool ConfirmPhoneNumber { get; set; }
  }
}
