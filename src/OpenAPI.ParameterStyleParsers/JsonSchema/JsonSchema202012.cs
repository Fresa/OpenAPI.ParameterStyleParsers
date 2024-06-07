using System.Collections.Immutable;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using OpenAPI.ParameterStyleParsers.Json;

namespace OpenAPI.ParameterStyleParsers.JsonSchema;

/// <inheritdoc />
public sealed class JsonSchema202012 : IJsonSchema
{
    private readonly Lazy<JsonNode?> _schemaResolver;
    private JsonNode? Schema => _schemaResolver.Value;
    private Lazy<InstanceType?>? _jsonValueType;
    private Lazy<IJsonSchema?>? _items;
    private Lazy<IReadOnlyDictionary<string, IJsonSchema>?>? _properties;
    private Lazy<IJsonSchema?>? _additionalProperties;
    private Lazy<IReadOnlyDictionary<Regex, IJsonSchema>?>? _patternProperties;

    /// <summary>
    /// Instantiate from a Json Schema draft 2020-12
    /// </summary>
    /// <param name="schema">Json Schema draft 2020-12</param>
    public JsonSchema202012(JsonNode? schema)
    {
        _schemaResolver = new Lazy<JsonNode?>(schema.ResolveRef);
    }

    /// <inheritdoc />
    public InstanceType? GetInstanceType()
    {
        _jsonValueType ??= new Lazy<InstanceType?>(() => (Schema as JsonObject)?["type"] switch
        {
            null => null,
            JsonArray array => array
                .Select(type =>
                    ParseType(type?.GetValue<string>()))
                .Aggregate((jsonValueTypes, jsonValueType) =>
                    jsonValueTypes | jsonValueType),
            JsonValue jsonValue => ParseType(jsonValue.GetValue<string>()),
            _ => throw new InvalidOperationException("Expected 'type' to be an array or string")
        });
        return _jsonValueType.Value;
    }

    private static InstanceType? ParseType(string? type) =>
        type switch
        {
            null => throw new InvalidOperationException("Expected type value but got null"),
            "" => throw new InvalidOperationException("Expected type value but got empty string"),
            "object" => InstanceType.Object,
            "array" => InstanceType.Array,
            "string" => InstanceType.String,
            "number" => InstanceType.Number,
            "integer" => InstanceType.Integer,
            "boolean" => InstanceType.Boolean,
            "null" => InstanceType.Null,
            _ => throw new InvalidOperationException($"type '{type}' is invalid")
        };

    /// <inheritdoc />
    public IJsonSchema? GetItems()
    {
        _items ??= new Lazy<IJsonSchema?>(() =>
        {
            var items = (Schema as JsonObject)?["items"];
            return items == null ? null : new JsonSchema202012(items);
        });
        return _items.Value;
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, IJsonSchema>? GetProperties()
    {
        _properties ??= new Lazy<IReadOnlyDictionary<string, IJsonSchema>?>(() =>
        {
            if ((Schema as JsonObject)?["properties"] is not JsonObject properties)
                return null;
            return properties.ToImmutableDictionary<KeyValuePair<string, JsonNode?>, string, IJsonSchema>(
                pair => pair.Key,
                pair => new JsonSchema202012(pair.Value));
        });
        return _properties.Value;
    }

    /// <inheritdoc />
    public IJsonSchema? GetAdditionalProperties()
    {
        _additionalProperties ??= new Lazy<IJsonSchema?>(() =>
        {
            var items = (Schema as JsonObject)?["additionalProperties"];
            return items == null ? null : new JsonSchema202012(items);
        });
        return _additionalProperties.Value;
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<Regex, IJsonSchema>? GetPatternProperties()
    {
        _patternProperties ??= new Lazy<IReadOnlyDictionary<Regex, IJsonSchema>?>(() =>
        {
            if ((Schema as JsonObject)?["patternProperties"] is not JsonObject properties)
                return null;
            return properties.ToImmutableDictionary<KeyValuePair<string, JsonNode?>, Regex, IJsonSchema>(
                pair => new Regex(pair.Key),
                pair => new JsonSchema202012(pair.Value));
        });
        return _patternProperties.Value;
    }
}