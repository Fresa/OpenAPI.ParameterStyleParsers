using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Array;

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
                return Explode ? [value] : value.Split(Delimiter);
            })
            .ToArray();
        return TryGetArrayItems(arrayValues, out array, out error);
    }

    public override bool ValueIncludesParameterName => true;
    public override string Delimiter => Explode ? ";" : ",";

    protected override string Serialize(string?[] values)
    {
        var serialized = string.Join(Delimiter,
            values.Select(value => Explode ? $"{ParameterName}{(string.IsNullOrEmpty(value) ? "" : "=")}{value}" : value));
        return $";{(Explode ? serialized : $"{ParameterName}={serialized}")}";
    }
}