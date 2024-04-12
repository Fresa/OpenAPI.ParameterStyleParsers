using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Array;

internal sealed class MatrixArrayValueParser(Parameter parameter) : ArrayValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var arrayValues = value?
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .SelectMany(expression =>
            {
                var valueAndKey = expression.Split('=');
                var value = valueAndKey.Length == 1 ? string.Empty : valueAndKey.Last();
                return Explode ? [value] : value.Split(',');
            })
            .ToArray();
        return TryGetArrayItems(arrayValues, out array, out error);
    }

    protected override string Serialize(string?[] values)
    {
        var serialized = string.Join(Explode ? ';' : ',',
            values.Select(value => Explode ? $"{parameter.Name}{(string.IsNullOrEmpty(value) ? "" : "=")}{value}" : value));
        return $";{(Explode ? serialized : $"{parameter.Name}={serialized}")}";
    }
}