using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Array;

internal sealed class CookieArrayValueParser : IValueParser
{
    private readonly InstanceType _jsonType;
    private readonly bool _explode;
    private readonly string _parameterName;

    internal CookieArrayValueParser(Parameter parameter)
    {
        _explode = parameter.Explode;
        _parameterName = parameter.Name;
        var itemsSchema = parameter.JsonSchema.GetItems();
        var jsonType = itemsSchema?.GetInstanceType();
        _jsonType = jsonType ?? InstanceType.String;
    }

    public bool TryParse(string? value, out JsonNode? array, [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            array = null;
            error = null;
            return true;
        }

        string[] arrayValues;
        if (_explode)
        {
            // Cookie style exploded: "name=val1; name=val2"
            arrayValues = value
                .Split("; ", StringSplitOptions.RemoveEmptyEntries)
                .Select(expression =>
                {
                    var parts = expression.Split('=', 2);
                    return parts.Length == 1 ? string.Empty : parts[1];
                })
                .ToArray();
        }
        else
        {
            // Cookie style non-exploded: "name=val1,val2"
            var parts = value.Split('=', 2);
            var valuesStr = parts.Length > 1 ? parts[1] : string.Empty;
            arrayValues = valuesStr.Split(',');
        }

        return TryGetArrayItems(arrayValues, out array, out error);
    }

    public string? Serialize(JsonNode? instance)
    {
        if (instance == null)
        {
            return null;
        }

        // Cookie style: no percent-encoding
        var values = instance
            .AsArray()
            .Select(node => node?.ToString())
            .ToArray();

        if (_explode)
        {
            return string.Join("; ", values.Select(v => $"{_parameterName}={v}"));
        }
        return $"{_parameterName}={string.Join(',', values)}";
    }

    private bool TryGetArrayItems(
        IReadOnlyList<string>? values,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        if (values == null || !values.Any())
        {
            error = null;
            array = null;
            return true;
        }

        var items = new JsonNode?[values.Count];
        for (var index = 0; index < values.Count; index++)
        {
            var arrayValue = values[index];
            if (!PrimitiveJsonConverter.TryConvert(arrayValue, _jsonType, out var item, out error))
            {
                array = null;
                return false;
            }
            items[index] = item;
        }

        error = null;
        array = new JsonArray(items);
        return true;
    }
}