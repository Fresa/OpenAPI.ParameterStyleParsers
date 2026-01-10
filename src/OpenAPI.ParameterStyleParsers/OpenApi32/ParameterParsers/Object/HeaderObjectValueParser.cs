using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Object;

internal sealed class HeaderObjectValueParser : IValueParser
{
    private readonly PropertySchemaResolver _propertySchemaResolver;
    private readonly bool _explode;

    internal HeaderObjectValueParser(Parameter parameter)
    {
        _propertySchemaResolver = new PropertySchemaResolver(parameter.JsonSchema);
        _explode = parameter.Explode;
    }

    public bool TryParse(string? value, out JsonNode? obj, [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            obj = null;
            error = null;
            return true;
        }

        // Simple style: comma-separated, no percent-decoding
        var keyAndValues = value.Split(',');
        if (_explode)
        {
            keyAndValues = keyAndValues
                .SelectMany(v => v.Split('='))
                .ToArray();
        }
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    public string? Serialize(JsonNode? instance)
    {
        if (instance == null)
        {
            return null;
        }

        // Simple style: comma-separated, no percent-encoding
        var obj = instance.AsObject();

        return string.Join(',',
            obj.Select(pair => $"{pair.Key}{(_explode ? "=" : ",")}{pair.Value}"));
    }

    private bool TryGetObjectProperties(
        IReadOnlyList<string>? keyAndValues,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (keyAndValues == null)
        {
            obj = null;
            error = null;
            return true;
        }

        var jsonObject = new JsonObject();
        for (var i = 0; i < keyAndValues.Count; i += 2)
        {
            var propertyName = keyAndValues[i];
            // No percent-decoding for headers
            var propertyValue = keyAndValues.Count == i + 1 ? string.Empty : keyAndValues[i + 1];

            _propertySchemaResolver.TryGetSchemaForProperty(propertyName, out var propertySchema);
            var jsonType = propertySchema?.GetInstanceType() ?? InstanceType.String;

            if (!PrimitiveJsonConverter.TryConvert(propertyValue, jsonType, out var jsonValue, out error))
            {
                obj = null;
                return false;
            }
            jsonObject[propertyName] = jsonValue;
        }

        obj = jsonObject;
        error = null;
        return true;
    }
}