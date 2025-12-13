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
        string GetWrongTypeError() => $"Value {value} is not a {jsonType}";
        bool TryParse([NotNullWhen(true)] out JsonNode? jsonNode)
        {
            try
            {
                jsonNode = JsonNode.Parse(value);
                return jsonNode != null;
            }
            catch (JsonException)
            {
                jsonNode = null;
                return false;
            }
        }
        
        switch (jsonType)
        {
            case Parameter.Types.String:
                instance = JsonValue.Create(value);
                error = null;
                return true;
            case Parameter.Types.Number:
                if (TryParse(out instance) &&
                    instance.GetValueKind() is JsonValueKind.Number)
                {
                    error = null;
                    return true;
                }

                instance = null;
                error = GetWrongTypeError();
                return false;
            case Parameter.Types.Boolean:
                if (TryParse(out instance) &&
                    instance.GetValueKind() is JsonValueKind.False or JsonValueKind.True)
                {
                    error = null;
                    return true;
                }

                instance = null;
                error = GetWrongTypeError();
                return false;
            case Parameter.Types.Integer:
                if (TryParse(out instance) &&
                    instance.GetValueKind() is JsonValueKind.Number &&
                    (value == "-0" || 
                     value.EndsWith(".0") || 
                     (long.TryParse(value, out var integer) && 
                      integer is <= 9007199254740992 and > -9007199254740992)))
                {
                    error = null;
                    return true;
                }

                instance = null;
                error = GetWrongTypeError();
                return false;
            default:
                error = $"Json type {jsonType} is not a primitive type, expected one of {string.Join(", ", Parameter.Types.Primitives)}";
                instance = null;
                return false;
        }
    }
}