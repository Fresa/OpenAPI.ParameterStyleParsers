using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Object;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;

internal sealed class MissingSchemaTypeValueParser : IValueParser
{
    private readonly IValueParser _valueParser;

    private MissingSchemaTypeValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
    }

    internal static MissingSchemaTypeValueParser Create(Parameter parameter)
    {
        if (parameter.Style == Parameter.Styles.DeepObject)
            return new MissingSchemaTypeValueParser(new DeepObjectValueParser(parameter));
        var arrayValueParser = ArrayValueParser.Create(parameter);
        return new MissingSchemaTypeValueParser(arrayValueParser);
    }

    public bool TryParse(string? value, out JsonNode? instance, 
        [NotNullWhen(false)] out string? error) =>
        _valueParser.TryParse(value, out instance, out error);

    public string? Serialize(JsonNode? instance) => 
        _valueParser.Serialize(instance);
}