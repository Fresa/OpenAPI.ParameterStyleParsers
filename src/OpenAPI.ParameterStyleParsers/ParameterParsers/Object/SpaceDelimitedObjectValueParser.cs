using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class SpaceDelimitedObjectValueParser(bool explode, JsonSchema schema)
    : ObjectValueParser(schema, explode)
{
    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (Explode)
        {
            error = "space delimited style with explode not supported for objects as the parameter name cannot be determined";
            obj = null;
            return false;
        }

        var keyAndValues = value?.Split("%20");
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }
}