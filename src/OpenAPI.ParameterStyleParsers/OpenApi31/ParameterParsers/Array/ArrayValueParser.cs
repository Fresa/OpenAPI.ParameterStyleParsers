using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Array;

internal abstract class ArrayValueParser(Parameter parameter) : IValueParser
{
    private readonly InstanceType _jsonType = 
        parameter.JsonSchema.GetItems()?.GetInstanceType() ?? 
        InstanceType.String;

    protected bool Explode { get; } = parameter.Explode;
    protected string ParameterName { get; } = parameter.Name;
    
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

    public abstract bool ValueIncludesParameterName { get; }

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
            var arrayValue = Uri.UnescapeDataString(values[index]);

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