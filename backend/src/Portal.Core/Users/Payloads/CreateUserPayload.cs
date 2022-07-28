﻿namespace Portal.Core.Users.Payloads
{
  public class CreateUserPayload : SaveUserPayload
  {
    public string? Realm { get; set; }

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
  }
}
