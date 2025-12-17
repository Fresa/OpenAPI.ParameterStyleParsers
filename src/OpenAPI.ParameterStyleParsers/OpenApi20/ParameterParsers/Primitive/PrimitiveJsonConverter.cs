using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

internal static class PrimitiveJsonConverter
{
    /// <summary>
    /// Converts a style parameter value to json
    /// </summary>
    /// <param name="value">Parameter styled value</param>
    /// <param name="jsonType">The expected json schema type</param>
    /// <param name="instance">The converted value.
    /// Note: The converted value might not correspond to <paramref name="jsonType"/>
    /// if <paramref name="value" /> isn't</param>
    /// <param name="error">An error if the value isn't convertable</param>
    /// <returns>True if the value was converted</returns>
    internal static bool TryConvert(
        string value,
        string jsonType,
        out JsonNode? instance,
        [NotNullWhen(false)] out string? error)
    {
        instance = jsonType switch
        {
            // Use string as-is if it is declared as a string.
            // "1.2" can be both a number and a string, but it's formatted differently in json format,
            // i.e. 1.2 or "1.2". We always want the latter if it is a string.
            Parameter.Types.String => JsonValue.Create(value),
            _ => Parse()
        };
        error = null;
        return true;

        JsonNode? Parse()
        {
            try
            {
                // The purpose of the converter is to convert parameter styled values
                // to json values not to validate that the value is of a specific type
                // or valid according to a schema, so we let it parse as-is
                return JsonNode.Parse(value);
            }
            // Value is not json formatted, so it will be treated as a string
            catch (JsonException)
            {
                return JsonValue.Create(value);
            }
        }
    }
}