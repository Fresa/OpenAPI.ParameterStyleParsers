using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace OpenAPI.ParameterStyleParsers;

/// <summary>
/// Parameter value parser factory
/// </summary>
[PublicAPI]
public static class ParameterValueParserFactory
{
    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static ParameterParsers.ParameterValueParser OpenApi31(JsonObject parameterSpecification) => 
        ParameterParsers.ParameterValueParser.FromOpenApi31ParameterSpecification(parameterSpecification);

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static ParameterParsers.ParameterValueParser OpenApi31(string parameterSpecificationAsJson)
    {
        var json = JsonNode.Parse(parameterSpecificationAsJson)?.AsObject() ??
                   throw new InvalidOperationException("Parameter specification is not a json object");
        return ParameterParsers.ParameterValueParser.FromOpenApi31ParameterSpecification(json);
    }

    /// <summary>
    /// Create a parameter value parser for OpenAPI 2.0
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi20.ParameterParsers.ParameterValueParser OpenApi20(JsonObject parameterSpecification) => 
        ParameterStyleParsers.OpenApi20.ParameterParsers.ParameterValueParser.Create(ParameterStyleParsers.OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterSpecification));

    /// <summary>
    /// Create a parameter value parser for OpenAPI 2.0
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi20.ParameterParsers.ParameterValueParser OpenApi20(string parameterSpecificationAsJson) => 
        ParameterStyleParsers.OpenApi20.ParameterParsers.ParameterValueParser.Create(ParameterStyleParsers.OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterSpecificationAsJson));
}