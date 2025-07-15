using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

internal sealed class MultiArrayValueParser(Parameter parameter) : ArrayValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var arrayValues = value?
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(expression =>
            {
                var valueAndKey = expression.Split('=');
                return valueAndKey.Length == 1 ? string.Empty : valueAndKey.Last();
            })
            .ToArray();
        return TryGetArrayItems(arrayValues, out array, out error);
    }

    protected override string Serialize(string?[] values) =>
        string.Join('&',
            values.Select(value => $"{ParameterName}={value}"));
}