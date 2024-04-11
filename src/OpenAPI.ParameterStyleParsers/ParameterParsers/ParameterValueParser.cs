using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

public sealed class ParameterValueParser : IParameterValueParser
{
    private readonly IValueParser _valueParser;

    private ParameterValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
    }

    public static ParameterValueParser Create(Parameter parameter)
    {
        var valueParser = CreateValueParser(parameter);
        return new ParameterValueParser(valueParser);
    }
    
    private static IValueParser CreateValueParser(Parameter parameter)
    {
        var jsonSchema = parameter.JsonSchema;
        var jsonType = jsonSchema.GetJsonType();

        return jsonType switch
        {
            null => MissingSchemaTypeValueParser.Create(parameter),
            SchemaValueType.String or
                SchemaValueType.Boolean or
                SchemaValueType.Integer or
                SchemaValueType.Number or
                SchemaValueType.Null
                => PrimitiveValueParser.Create(parameter, jsonSchema),
            SchemaValueType.Array => ArrayValueParser.Create(parameter, jsonSchema),
            SchemaValueType.Object => ObjectValueParser.Create(parameter, jsonSchema),
            _ => throw new NotSupportedException($"Json type {Enum.GetName(jsonType.Value)} is not supported")
        };
    }

    public bool TryParse(string? value, out JsonNode? instance,
        [NotNullWhen(false)] out string? mappingError) =>
        _valueParser.TryParse(value, out instance, out mappingError);
}
