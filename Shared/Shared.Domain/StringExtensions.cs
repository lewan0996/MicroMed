namespace Shared.Domain;

public static class StringExtensions
{
    public static bool IsNullOrEmptyOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);
}