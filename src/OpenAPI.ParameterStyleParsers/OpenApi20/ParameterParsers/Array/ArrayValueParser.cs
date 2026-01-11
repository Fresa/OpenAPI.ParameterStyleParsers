using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

internal abstract class ArrayValueParser : IValueParser
{
    private readonly string _itemType;

    protected string ParameterName { get; }
    public bool ValueIncludesParameterName { get; }

    protected ArrayValueParser(Parameter parameter)
    {
        if (!parameter.IsArray)
        {
            throw new InvalidOperationException($"The parameter is not an array type but '{parameter.Type}'");
        }

        ParameterName = parameter.Name;
        _itemType = parameter.Items.Type;
        ValueIncludesParameterName = parameter.ValueIncludesKey;
    }

    internal static ArrayValueParser Create(Parameter parameter) =>
        parameter.CollectionFormat switch
        {
            Parameter.CollectionFormats.Csv => new CsvArrayValueParser(parameter),
            Parameter.CollectionFormats.Pipes => new PipesArrayValueParser(parameter),
            Parameter.CollectionFormats.Multi => new MultiArrayValueParser(parameter),
            Parameter.CollectionFormats.Ssv => new SsvArrayValueParser(parameter),
            Parameter.CollectionFormats.Tsv => new TsvArrayValueParser(parameter),
            _ => throw new ArgumentException(nameof(parameter.CollectionFormat),
                $"Unknown collection format '{parameter.CollectionFormat}'")
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

            if (!PrimitiveJsonConverter.TryConvert(arrayValue, _itemType, out var item, out error))
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