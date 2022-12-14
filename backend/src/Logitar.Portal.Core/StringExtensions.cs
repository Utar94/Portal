namespace Logitar.Portal.Core
{
  public static class StringExtensions
  {
    public static string? CleanTrim(this string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    public static string Remove(this string s, string pattern)
      => s?.Replace(pattern, string.Empty) ?? throw new ArgumentNullException(nameof(s));
  }
}
