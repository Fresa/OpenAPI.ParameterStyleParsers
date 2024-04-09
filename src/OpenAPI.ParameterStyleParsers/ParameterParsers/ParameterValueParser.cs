using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

internal sealed class ParameterValueParser : IParameterValueParser
{
    private readonly IValueParser _valueParser;

    private ParameterValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
    }

    internal static ParameterValueParser Create(Parameter parameter, JsonSchema jsonSchema)
    {
        var valueParser = CreateValueParser(parameter, jsonSchema);
        return new ParameterValueParser(valueParser);
    }
    private static IValueParser CreateValueParser(Parameter parameter, JsonSchema jsonSchema)
    {
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

    public bool TryParse(string[] values, out JsonNode? instance,
        [NotNullWhen(false)] out string? mappingError) =>
        _valueParser.TryParse(values, out instance, out mappingError);
}