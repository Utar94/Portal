using Logitar.Portal.Application.Emails.Messages;
using Logitar.Portal.Domain.Users;

namespace Logitar.Portal.Infrastructure.Emails.Messages
{
  public class TemplateModel
  {
    private readonly Dictionaries _dictionaries;
    private readonly IReadOnlyDictionary<string, string?> _variables;

    public TemplateModel(
      Dictionaries? dictionaries = null,
      User? user = null,
      IReadOnlyDictionary<string, string?>? variables = null
    )
    {
      _dictionaries = dictionaries ?? new();
      User = user;
      _variables = variables ?? new Dictionary<string, string?>();
    }

    public User? User { get; }

    public string Resource(string key) => _dictionaries.GetEntry(key);
    public string? Variable(string key) => _variables.TryGetValue(key, out string? value) ? value : null;
  }
}
