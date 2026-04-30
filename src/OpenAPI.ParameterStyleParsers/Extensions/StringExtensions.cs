namespace OpenAPI.ParameterStyleParsers.Extensions;

internal static class StringExtensions
{
    /// <summary>
    /// Splits a string using a separator while honoring quoted separators
    /// </summary>
    /// <param name="value">String to be split</param>
    /// <param name="separator">Separator</param>
    /// <returns>A list of separated items</returns>
    internal static List<string> SplitWithQuotation(this string value, char separator)
    {
        if (separator == '"')
            throw new ArgumentException("Cannot split with the quote character", nameof(separator));
        var result = new List<string>();
        var inQuotes = false;
        var start = 0;
        for (var i = 0; i < value.Length; i++)
        {
            switch (value[i])
            {
                case '"' when inQuotes && i > 0 && value[i-1] == '\\':
                    break;
                case '"':
                    inQuotes = !inQuotes;
                    break;
                case var chr when !inQuotes && chr == separator:
                    result.Add(value[start..i]);
                    start = i + 1;
                    break;
            }
        }
        result.Add(value[start..]);
        return result;
    }
}