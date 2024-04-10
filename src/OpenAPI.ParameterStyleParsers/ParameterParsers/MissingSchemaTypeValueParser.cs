using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

internal sealed class MissingSchemaTypeValueParser : IValueParser
{
    private readonly IValueParser _valueParser;

    private MissingSchemaTypeValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
    }

    internal static MissingSchemaTypeValueParser Create(Parameter parameter)
    {
        var stringSchema = new JsonSchemaBuilder().Type(SchemaValueType.String);
        if (parameter.Style == Parameter.Styles.DeepObject)
            return new MissingSchemaTypeValueParser(new DeepObjectValueParser(parameter.Explode, stringSchema));
        var arrayValueParser = ArrayValueParser.Create(parameter,
            new JsonSchemaBuilder().Type(SchemaValueType.Array).Items(stringSchema));
        return new MissingSchemaTypeValueParser(arrayValueParser);
    }

    public bool TryParse(string? value, out JsonNode? instance, 
        [NotNullWhen(false)] out string? error)
    {
        return _valueParser.TryParse(value, out instance, out error);
    }
}