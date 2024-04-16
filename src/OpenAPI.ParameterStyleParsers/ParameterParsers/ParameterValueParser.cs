using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

public sealed class ParameterValueParser
{
    private readonly IValueParser _valueParser;

    private ParameterValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
    }

    /// <summary>
    /// Creates a parameter value parser corresponding to the specified parameter
    /// </summary>
    /// <param name="parameter">The parameter specification</param>
    /// <returns>Parameter value parser</returns>
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
                => PrimitiveValueParser.Create(parameter),
            SchemaValueType.Array => ArrayValueParser.Create(parameter),
            SchemaValueType.Object => ObjectValueParser.Create(parameter),
            _ => throw new NotSupportedException($"Json type {Enum.GetName(jsonType.Value)} is not supported")
        };
    }

    /// <summary>
    /// Parses a style formatted parameter value to a json node.
    /// It's assumed that the input is valid according to the style format.
    /// </summary>
    /// <param name="value">Style formatted input</param>
    /// <param name="instance">The parsed json if this method returns true</param>
    /// <param name="error">Parsing error if this method returns false</param>
    /// <returns>true if an instance could be constructed, false if there are errors</returns>
    public bool TryParse(string? value, out JsonNode? instance,
        [NotNullWhen(false)] out string? error) =>
        _valueParser.TryParse(value, out instance, out error);

    /// <summary>
    /// Serializes a json node according to the specified parameter.
    /// It's assumed that the instance is valid according to the parameter's schema.
    /// </summary>
    /// <param name="instance">Json instance</param>
    /// <returns>Style formatted instance</returns>
    public string? Serialize(JsonNode? instance) => 
        _valueParser.Serialize(instance);
}
