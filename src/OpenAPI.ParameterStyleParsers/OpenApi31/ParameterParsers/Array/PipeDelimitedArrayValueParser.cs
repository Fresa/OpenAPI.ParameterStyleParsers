using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Array;

internal sealed class PipeDelimitedArrayValueParser(Parameter parameter) : ArrayValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var arrayValues = value?
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .SelectMany(expression =>
            {
                var valueAndKey = expression.Split('=');
                var value = valueAndKey.Length == 1 ? string.Empty : valueAndKey.Last();
                return Explode ? [value] : value.Split('|');
            })
            .ToArray();

        return TryGetArrayItems(arrayValues, out array, out error);
    }

    public override bool ValueIncludesParameterName => !Explode;

    protected override string Serialize(string?[] values)
    {
        var serialized = string.Join(Explode ? '&' : '|',
            values.Select(value => Explode ? $"{ParameterName}={value}" : value));
        return Explode ? serialized : $"{ParameterName}={serialized}";
    }
}