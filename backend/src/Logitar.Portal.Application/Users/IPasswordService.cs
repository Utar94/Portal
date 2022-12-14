using Logitar.Portal.Domain.Realms;
using Logitar.Portal.Domain.Users;

namespace Logitar.Portal.Application.Users
{
  public interface IPasswordService
  {
    string GenerateAndHash(int length, out byte[] password);
    string Hash(string password);
    bool IsMatch(User user, string password);
    bool IsMatch(string hash, byte[] password);
    void ValidateAndThrow(string password, Realm? realm = null);
  }
}
