using System.Text.Json.Nodes;
using JetBrains.Annotations;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers;

/// <summary>
/// Parameter value parser factories
/// </summary>
[PublicAPI]
public static class ParameterValueParsers
{
    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static ParameterValueParser ForOpenApi31(JsonObject parameterSpecification) => 
        ParameterValueParser.FromOpenApi31ParameterSpecification(parameterSpecification);

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static ParameterValueParser ForOpenApi31(string parameterSpecificationAsJson)
    {
        var json = JsonNode.Parse(parameterSpecificationAsJson)?.AsObject() ??
                   throw new InvalidOperationException("Parameter specification is not a json object");
        return ParameterValueParser.FromOpenApi31ParameterSpecification(json);
    }

    /// <summary>
    /// Create a parameter value parser for OpenAPI 2.0
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi20.ParameterParsers.ParameterValueParser ForOpenApi20(JsonObject parameterSpecification) => 
        OpenApi20.ParameterParsers.ParameterValueParser.Create(OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterSpecification));

    /// <summary>
    /// Create a parameter value parser for OpenAPI 2.0
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi20.ParameterParsers.ParameterValueParser ForOpenApi20(string parameterSpecificationAsJson) => 
        OpenApi20.ParameterParsers.ParameterValueParser.Create(OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterSpecificationAsJson));
}