using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Array;

internal abstract class ArrayValueParser : IValueParser
{
    private readonly SchemaValueType _jsonType;

    internal bool Explode { get; }
    protected ArrayValueParser(JsonSchema schema, bool explode)
    {
        Explode = explode;
        var itemsSchema = schema.GetItems();
        if (itemsSchema == null)
        {
            throw new ArgumentException("Missing 'items' keyword for array schema");
        }
        var jsonType = itemsSchema.GetJsonType();
        if (jsonType == null)
        {
            throw new ArgumentException("Missing 'type' attribute for array schema items");
        }

        _jsonType = jsonType.Value;
    }
    
    internal static ArrayValueParser Create(Parameter parameter, JsonSchema schema) =>
        parameter.Style switch
        {
            Parameter.Styles.Matrix => new MatrixArrayValueParser(parameter.Explode, schema),
            Parameter.Styles.Label => new LabelArrayValueParser(parameter.Explode, schema),
            Parameter.Styles.Form => new FormArrayValueParser(parameter.Explode, schema),
            Parameter.Styles.Simple => new SimpleArrayValueParser(parameter.Explode, schema),
            Parameter.Styles.SpaceDelimited => new SpaceDelimitedArrayValueParser(parameter.Explode, schema),
            Parameter.Styles.PipeDelimited => new PipeDelimitedArrayValueParser(parameter.Explode, schema),
            _ => throw new ArgumentException(nameof(parameter.Style),
                $"Style '{parameter.Style}' does not support arrays")
        };

    public abstract bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error);

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