using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Array;

internal sealed class HeaderArrayValueParser : IValueParser
{
    private readonly InstanceType _jsonType;

    internal HeaderArrayValueParser(Parameter parameter)
    {
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

        // Simple style: comma-separated, no percent-decoding
        var arrayValues = value.Split(',');
        return TryGetArrayItems(arrayValues, out array, out error);
    }

    public string? Serialize(JsonNode? instance)
    {
        if (instance == null)
        {
            return null;
        }

        // Simple style: comma-separated, no percent-encoding
        var values = instance
            .AsArray()
            .Select(node => node?.ToString());

        return string.Join(',', values);
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