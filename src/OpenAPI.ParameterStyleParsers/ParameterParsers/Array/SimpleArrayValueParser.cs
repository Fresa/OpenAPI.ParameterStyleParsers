using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Array;

internal sealed class SimpleArrayValueParser(bool explode, JsonSchema schema) : ArrayValueParser(schema, explode)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var arrayValues = value?
            .Split(',');
        return TryGetArrayItems(arrayValues, out array, out error);
    }
}