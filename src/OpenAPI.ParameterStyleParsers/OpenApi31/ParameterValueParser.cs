using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi31;

/// <summary>
/// Represents a parameter value parser for OpenAPI 3.1 styles
/// </summary>
public sealed class ParameterValueParser : IParameterValueParser
{
    private readonly IValueParser _valueParser;

    private ParameterValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
        ValueIncludesParameterName = valueParser.ValueIncludesParameterName;
    }

    /// <inheritdoc />
    public bool ValueIncludesParameterName { get; }

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

    /// <summary>
    /// Create a parameter value parser from an OpenAPI 3.1 parameter specification
    /// <see href="https://spec.openapis.org/oas/v3.1.0#parameter-object"/>
    /// </summary>
    /// <param name="parameterSpecification">Specification of the parameter</param>
    /// <returns>Parameter value parser</returns>
    /// <exception cref="InvalidOperationException">The provided json object doesn't correspond to the specification</exception>
    public static ParameterValueParser FromOpenApi31ParameterSpecification(JsonObject parameterSpecification) =>
        Create(Parameter.FromOpenApi31ParameterSpecification(parameterSpecification));

    private static IValueParser CreateValueParser(Parameter parameter)
    {
        var jsonSchema = parameter.JsonSchema;
        var jsonType = jsonSchema.GetInstanceType();

        return jsonType switch
        {
            null => MissingSchemaTypeValueParser.Create(parameter),
            { } t when t.HasFlag(InstanceType.String) ||
                       t.HasFlag(InstanceType.Boolean) ||
                       t.HasFlag(InstanceType.Integer) ||
                       t.HasFlag(InstanceType.Number) ||
                       t == InstanceType.Null => PrimitiveValueParser.Create(parameter),
            { } t when t.HasFlag(InstanceType.Array) => ArrayValueParser.Create(parameter),
            { } t when t.HasFlag(InstanceType.Object) => ObjectValueParser.Create(parameter),
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