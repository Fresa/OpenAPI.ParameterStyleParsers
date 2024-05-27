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

    internal static JsonNode? ResolveRef(this JsonNode? jsonNode)
    {
        switch (jsonNode)
        {
            case JsonObject jsonObject:
                if (!jsonObject.TryGetPropertyValue("$ref", out var refNode))
                    return jsonNode;
                if (refNode is null)
                {
                    throw new InvalidOperationException(
                        $"JsonObject with null $ref property found at {jsonNode.GetPath()}");
                }
                var reference = refNode.GetValue<string>();
                return jsonNode.Root.Resolve(JsonPointer.Parse(reference));
            default:
                return jsonNode;
        }
    }

    internal static JsonNode? Resolve(this JsonNode? jsonNode, JsonPointer jsonPointer)
    {
        var currentNode = jsonNode;
        foreach (var segment in jsonPointer.Segments)
        {
            switch (currentNode)
            {
                case JsonObject currentObject:
                    if (!currentObject.TryGetPropertyValue(segment, out currentNode))
                    {
                        throw new InvalidOperationException(
                            $"Json object at path {currentObject.GetPath()} does not have a property named {segment}");
                    }

                    break;
                case JsonArray currentArray:
                    if (!int.TryParse(segment, out var index) || index < 0)
                    {
                        throw new InvalidOperationException(
                            $"Json array at path {currentArray.GetPath()} is referenced with an invalid index: {segment}");
                    }

                    if (index > currentArray.Count)
                    {
                        throw new IndexOutOfRangeException(
                            $"Json array at path {currentArray.GetPath()} is referenced out of range: {segment}");
                    }

                    currentNode = currentArray[index];
                    break;
                case JsonValue:
                    throw new InvalidOperationException(
                        $"Expected json value at path {currentNode.GetPath()} to be an object or array with member {segment}");
                default:
                    throw new InvalidOperationException("Unknown json node");
            }
        }

        return currentNode;
    }
}