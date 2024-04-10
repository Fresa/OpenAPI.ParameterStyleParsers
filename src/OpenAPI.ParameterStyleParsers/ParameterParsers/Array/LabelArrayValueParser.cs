using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Array;

internal sealed class LabelArrayValueParser(bool explode, JsonSchema schema) : ArrayValueParser(schema, explode)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var arrayValues = value?
            .Split('.')[1..];
        return TryGetArrayItems(arrayValues, out array, out error);
    }
}