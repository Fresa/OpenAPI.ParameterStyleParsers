using System.Collections.Concurrent;
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
        IReadOnlyCollection<string> values,
        [NotNullWhen(true)] out JsonNode? array,
        [NotNullWhen(false)] out string? error);

    protected bool TryGetArrayItems(
        IReadOnlyList<string> values,
        [NotNullWhen(true)] out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
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

public record Parameter
{
    private static readonly ConcurrentDictionary<(string Style, bool Explode), Parameter> Parameters = new();

    public static class Styles
    {
        public const string Matrix = "matrix";
        public const string Label = "label";
        public const string Simple = "simple";
        public const string Form = "form";
        public const string SpaceDelimited = "spaceDelimited";
        public const string PipeDelimited = "pipeDelimited";
        public const string DeepObject = "deepObject";
    } 

    private static Parameter Get(string style, bool explode) =>
        Parameters.GetOrAdd((style, explode), input => new Parameter
        {
            Style = input.Style,
            Explode = input.Explode
        });

    public static Parameter Parse(string style, bool explode) =>
        style switch
        {
            Styles.Matrix => Matrix(explode),
            Styles.Label => Label(explode),
            Styles.Simple => Simple(explode),
            Styles.Form => Form(explode),
            Styles.SpaceDelimited => SpaceDelimited(explode),
            Styles.PipeDelimited => PipeDelimited(explode),
            Styles.DeepObject => DeepObject(explode),
            _ => throw new NotSupportedException($"Style {style} is not supported")
        };
    public static Parameter Matrix(bool explode) => 
        Get(Styles.Matrix, explode);
    public static Parameter Label(bool explode) =>
        Get(Styles.Label, explode);
    public static Parameter Simple(bool explode) =>
        Get(Styles.Simple, explode);
    public static Parameter Form(bool explode) =>
        Get(Styles.Form, explode);
    public static Parameter SpaceDelimited(bool explode) =>
        Get(Styles.SpaceDelimited, explode);
    public static Parameter PipeDelimited(bool explode) =>
        Get(Styles.PipeDelimited, explode);
    public static Parameter DeepObject(bool explode) =>
        Get(Styles.DeepObject, explode);

    public string Style { get; private init; }
    public bool Explode { get; private init; }
}
