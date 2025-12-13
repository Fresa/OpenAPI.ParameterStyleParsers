using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

internal static class PrimitiveJsonConverter
{
    internal static bool TryConvert(
        string value,
        string jsonType,
        out JsonNode? instance,
        [NotNullWhen(false)] out string? error)
    {
        switch (jsonType)
        {
            case Parameter.Types.String:
                instance = JsonValue.Create(value);
                error = null;
                return true;
            case Parameter.Types.Number:
            case Parameter.Types.Boolean:
            case Parameter.Types.Integer:
                try
                {
                    instance = JsonNode.Parse(value);
                    error = null;
                    return true;
                }
                catch (JsonException)
                {
                    instance = null;
                    error = $"Value {value} is not a {jsonType}";
                    return false;
                }
            default:
                error = $"Json type {jsonType} is not a primitive type, expected one of {string.Join(", ", Parameter.Types.Primitives)}";
                instance = null;
                return false;
        }
    }
}