using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class PipeDelimitedObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        [NotNullWhen(true)] out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (Explode)
        {
            error = "pipe delimited style with explode not supported for objects as the parameter name cannot be determined";
            obj = null;
            return false;
        }

        var keyAndValues = value?.Split('|');
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }
}