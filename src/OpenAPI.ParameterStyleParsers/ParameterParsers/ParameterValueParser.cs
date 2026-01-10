using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

/// <summary>
/// Represents a parameter value parser for OpenAPI 3.1 styles
/// </summary>
[Obsolete("Use OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.ParameterValueParser instead")]
public sealed class ParameterValueParser : IParameterValueParser
{
    private readonly OpenApi31.ParameterValueParser _innerParser;

    private ParameterValueParser(OpenApi31.ParameterValueParser innerParser)
    {
        _innerParser = innerParser;
    }

    /// <summary>
    /// Creates a parameter value parser corresponding to the specified parameter
    /// </summary>
    /// <param name="parameter">The parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static ParameterValueParser Create(Parameter parameter)
    {
        var innerParser = OpenApi31.ParameterValueParser.Create(parameter.Inner);
        return new ParameterValueParser(innerParser);
    }

    /// <summary>
    /// Create a parameter value parser from an OpenAPI 3.1 parameter specification
    /// <see href="https://spec.openapis.org/oas/v3.1.0#parameter-object"/>
    /// </summary>
    /// <param name="parameterSpecification">Specification of the parameter</param>
    /// <returns>Parameter value parser</returns>
    /// <exception cref="InvalidOperationException">The provided json object doesn't correspond to the specification</exception>
    public static ParameterValueParser FromOpenApi31ParameterSpecification(JsonObject parameterSpecification)
    {
        var innerParser = OpenApi31.ParameterValueParser.FromOpenApi31ParameterSpecification(parameterSpecification);
        return new ParameterValueParser(innerParser);
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
        _innerParser.TryParse(value, out instance, out error);

    /// <summary>
    /// Serializes a json node according to the specified parameter.
    /// It's assumed that the instance is valid according to the parameter's schema.
    /// </summary>
    /// <param name="instance">Json instance</param>
    /// <returns>Style formatted instance</returns>
    public string? Serialize(JsonNode? instance) => 
        _innerParser.Serialize(instance);
}