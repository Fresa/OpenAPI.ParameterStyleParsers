using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Object;

internal sealed class CookieObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    private readonly PropertySchemaResolver _propertySchemaResolver = new(parameter.JsonSchema);

    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            obj = null;
            error = null;
            return true;
        }

        if (Explode)
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

        // Cookie style non-exploded: "name=key1,value1,key2,value2"
        var nameValue = value.Split('=', 2);
        if (nameValue.Length < 2)
        {
            obj = null;
            error = "Expected name=value format";
            return false;
        }

        var keyAndValues = nameValue[1].Split(',');
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> properties) => Explode
        ? string.Join("; ", properties.Select(p => $"{p.Key}={p.Value}"))
        : $"{ParameterName}={string.Join(',', properties.Select(p => $"{p.Key},{p.Value}"))}";
}