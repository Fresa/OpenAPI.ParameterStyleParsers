using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Object;

internal sealed class CookieObjectValueParser : IValueParser
{
    private readonly PropertySchemaResolver _propertySchemaResolver;
    private readonly bool _explode;
    private readonly string _parameterName;

    internal CookieObjectValueParser(Parameter parameter)
    {
        _propertySchemaResolver = new PropertySchemaResolver(parameter.JsonSchema);
        _explode = parameter.Explode;
        _parameterName = parameter.Name;
    }

    public bool TryParse(string? value, out JsonNode? obj, [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            obj = null;
            error = null;
            return true;
        }

        if (_explode)
        {
            // Cookie style exploded: "key1=value1; key2=value2"
            var pairs = value.Split("; ", StringSplitOptions.RemoveEmptyEntries);
            var jsonObject = new JsonObject();

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=', 2);
                var propertyName = keyValue[0];
                var propertyValue = keyValue.Length > 1 ? keyValue[1] : string.Empty;

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
        else
        {
            // Cookie style non-exploded: "name=key1,value1,key2,value2"
            var keyValue = value.Split('=', 2);
            if (keyValue.Length < 2)
            {
                obj = null;
                error = "Expected name=value format";
                return false;
            }

            var keyAndValues = keyValue[1].Split(',');
            return TryGetObjectProperties(keyAndValues, out obj, out error);
        }
    }

    public string? Serialize(JsonNode? instance)
    {
        if (instance == null)
        {
            return null;
        }

        // Cookie style: no percent-encoding
        // JsonObject preserves insertion order
        var obj = instance.AsObject();

        if (_explode)
        {
            return string.Join("; ", obj.Select(p => $"{p.Key}={p.Value}"));
        }
        return $"{_parameterName}={string.Join(',', obj.Select(p => $"{p.Key},{p.Value}"))}";
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