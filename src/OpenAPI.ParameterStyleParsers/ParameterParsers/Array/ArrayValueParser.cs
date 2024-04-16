using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Array;

internal abstract class ArrayValueParser : IValueParser
{
    private readonly SchemaValueType _jsonType;

    protected bool Explode { get; }
    protected string ParameterName { get; }
    protected ArrayValueParser(Parameter parameter)
    {
        Explode = parameter.Explode;
        ParameterName = parameter.Name;
        var itemsSchema = parameter.JsonSchema.GetItems();
        var jsonType = itemsSchema?.GetJsonType();
        
        _jsonType = jsonType ?? SchemaValueType.String;
    }
    
    internal static ArrayValueParser Create(Parameter parameter) =>
        parameter.Style switch
        {
            Parameter.Styles.Matrix => new MatrixArrayValueParser(parameter),
            Parameter.Styles.Label => new LabelArrayValueParser(parameter),
            Parameter.Styles.Form => new FormArrayValueParser(parameter),
            Parameter.Styles.Simple => new SimpleArrayValueParser(parameter),
            Parameter.Styles.SpaceDelimited => new SpaceDelimitedArrayValueParser(parameter),
            Parameter.Styles.PipeDelimited => new PipeDelimitedArrayValueParser(parameter),
            _ => throw new ArgumentException(nameof(parameter.Style),
                $"Style '{parameter.Style}' does not support arrays")
        };

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

        var values = instance
            .AsArray()
            .Select(node => node == null ? null : Uri.EscapeDataString(node.ToString()))
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