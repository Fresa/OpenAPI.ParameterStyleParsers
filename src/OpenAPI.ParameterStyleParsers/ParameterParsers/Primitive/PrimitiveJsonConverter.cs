using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal static class PrimitiveJsonConverter
{
    internal static bool TryConvert(
        string? value,
        InstanceType jsonType,
        out JsonNode? instance,
        [NotNullWhen(false)] out string? error)
    {
        switch (jsonType)
        {
            case InstanceType.String:
                instance = JsonValue.Create(value);
                error = null;
                return true;
            case InstanceType.Number:
            case InstanceType.Boolean:
            case InstanceType.Integer:
            case InstanceType.Null:
                instance = value == null ? null : JsonNode.Parse(value);
                error = null;
                return true;
            default:
                error = $"Json type {Enum.GetName(jsonType)} is not a primitive type";
                instance = null;
                return false;
        }
    }
}