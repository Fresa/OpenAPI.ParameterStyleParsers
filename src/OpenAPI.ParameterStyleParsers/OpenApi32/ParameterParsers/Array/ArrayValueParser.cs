using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Array;

internal abstract class ArrayValueParser(Parameter parameter) : IValueParser
{
    private readonly InstanceType _jsonType =
        parameter.JsonSchema.GetItems()?.GetInstanceType() ??
        InstanceType.String;

    protected bool Explode { get; } = parameter.Explode;
    protected string ParameterName { get; } = parameter.Name;

    public abstract bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error);

    public string? Serialize(JsonNode? instance)
    {
        if (instance == null)
        {
            return null;
        }

        // No percent-encoding for OpenAPI 3.2 simple/cookie styles
        var values = instance
            .AsArray()
            .Select(node => node?.ToString())
            .ToArray();
        return Serialize(values);
    }

    protected abstract string Serialize(string?[] values);

    protected bool TryGetArrayItems(
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
            // No percent-decoding for OpenAPI 3.2 simple/cookie styles
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