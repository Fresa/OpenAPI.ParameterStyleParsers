using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class MatrixObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        var keyAndValues = value?
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .SelectMany(expression =>
            {
                var valueAndKey = expression.Split('=');
                var key = valueAndKey[0];
                var value = valueAndKey.Length == 1 ? string.Empty : valueAndKey.Last();
                return Explode ? [key, value] : value.Split(',');
            })
            .ToArray();

        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }
}