using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.Json;

internal static class JsonNodeExtensions
{
    internal static JsonNode? GetRequiredPropertyValue(this JsonObject json, string propertyName) =>
        !json.TryGetPropertyValue(propertyName, out var value)
            ? throw new InvalidOperationException($"Property '{propertyName}' is missing")
            : value;

    internal static T GetRequiredPropertyValue<T>(this JsonObject json, string propertyName)
    {
        var property = json.GetRequiredPropertyValue(propertyName);
        return property == null
            ? throw new InvalidOperationException($"Property '{propertyName}' is null")
            : property.GetValue<T>();
    }
}