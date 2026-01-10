using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers;

/// <summary>
/// Represents a parameter value parser for OpenAPI 3.2 styles
/// </summary>
[PublicAPI]
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
    [PublicAPI]
    public static ParameterValueParser Create(Parameter parameter)
    {
        var valueParser = CreateValueParser(parameter);
        return new ParameterValueParser(valueParser);
    }

    private static IValueParser CreateValueParser(Parameter parameter)
    {
        // Handle cookie style (new in 3.2) directly
        if (parameter.Style == Parameter.Styles.Cookie)
        {
            return CreateCookieStyleParser(parameter);
        }

        // For other styles, delegate to shared parsers via shared Parameter
        var sharedParameter = ParameterStyleParsers.Parameter.Parse(
            parameter.Name,
            parameter.Style,
            parameter.Location,
            parameter.Explode,
            parameter.JsonSchema);

        return CreateSharedValueParser(sharedParameter);
    }

    private static IValueParser CreateCookieStyleParser(Parameter parameter)
    {
        var jsonType = parameter.JsonSchema.GetInstanceType();

        return jsonType switch
        {
            null or
                InstanceType.String or
                InstanceType.Boolean or
                InstanceType.Integer or
                InstanceType.Number or
                InstanceType.Null => new CookiePrimitiveValueParser(parameter),
            InstanceType.Array => new CookieArrayValueParser(parameter),
            InstanceType.Object => new CookieObjectValueParser(parameter),
            _ => throw new NotSupportedException($"Json type {Enum.GetName(jsonType.Value)} is not supported")
        };
    }

    private static IValueParser CreateSharedValueParser(ParameterStyleParsers.Parameter parameter)
    {
        var jsonSchema = parameter.JsonSchema;
        var jsonType = jsonSchema.GetInstanceType();

        return jsonType switch
        {
            null => MissingSchemaTypeValueParser.Create(parameter),
            InstanceType.String or
                InstanceType.Boolean or
                InstanceType.Integer or
                InstanceType.Number or
                InstanceType.Null => PrimitiveValueParser.Create(parameter),
            InstanceType.Array => ArrayValueParser.Create(parameter),
            InstanceType.Object => ObjectValueParser.Create(parameter),
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
    [PublicAPI]
    public bool TryParse(string? value, out JsonNode? instance,
        [NotNullWhen(false)] out string? error) =>
        _valueParser.TryParse(value, out instance, out error);

    /// <summary>
    /// Serializes a json node according to the specified parameter.
    /// It's assumed that the instance is valid according to the parameter's schema.
    /// </summary>
    /// <param name="instance">Json instance</param>
    /// <returns>Style formatted instance</returns>
    [PublicAPI]
    public string? Serialize(JsonNode? instance) =>
        _valueParser.Serialize(instance);
}