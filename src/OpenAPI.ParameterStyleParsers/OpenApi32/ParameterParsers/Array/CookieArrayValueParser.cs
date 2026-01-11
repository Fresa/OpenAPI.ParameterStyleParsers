using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Array;

internal sealed class CookieArrayValueParser(Parameter parameter) : ArrayValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            array = null;
            error = null;
            return true;
        }

        string[] arrayValues;
        if (Explode)
        {
            arrayValues = value
                .Split("; ", StringSplitOptions.RemoveEmptyEntries)
                .Select(expression =>
                {
                    var parts = expression.Split('=', 2);
                    return parts.Length == 1 ? string.Empty : parts[1];
                })
                .ToArray();
        }
        else
        {
            var parts = value.Split('=', 2);
            var valuesStr = parts.Length > 1 ? parts[1] : string.Empty;
            arrayValues = valuesStr.Split(',');
        }

        return TryGetArrayItems(arrayValues, out array, out error);
    }

    protected override string Serialize(string?[] values) =>
        Explode
            ? string.Join("; ", values.Select(v => $"{ParameterName}={v}"))
            : $"{ParameterName}={string.Join(',', values)}";
}