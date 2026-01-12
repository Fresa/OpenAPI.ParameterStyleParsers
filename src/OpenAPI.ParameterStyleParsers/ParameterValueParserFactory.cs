using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace OpenAPI.ParameterStyleParsers;

/// <summary>
/// Parameter value parser factory
/// </summary>
[PublicAPI]
public static class ParameterValueParserFactory
{
    private static readonly Dictionary<string, Func<JsonObject, IParameterValueParser>> Factories = new()
    {
        ["2"] = OpenApi20,
        ["2.0"] = OpenApi20,
        ["3"] = OpenApi32,
        ["3.0"] = OpenApi30,
        ["3.0.0"] = OpenApi30,
        ["3.0.1"] = OpenApi30,
        ["3.0.2"] = OpenApi30,
        ["3.0.3"] = OpenApi30,
        ["3.0.4"] = OpenApi30,
        ["3.1"] = OpenApi31,
        ["3.1.0"] = OpenApi31,
        ["3.1.1"] = OpenApi31,
        ["3.1.2"] = OpenApi31,
        ["3.2"] = OpenApi32,
        ["3.2.0"] = OpenApi32
    };

    /// <summary>
    /// Create a parameter value parser for the specified OpenAPI version
    /// </summary>
    /// <param name="version">OpenAPI version (e.g., "2", "2.0", "3.1", "3.1.0" etc)</param>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    /// <exception cref="NotSupportedException">The specified version is not supported</exception>
    public static IParameterValueParser OpenApi(string version, JsonObject parameterSpecification)
    {
        if (!Factories.TryGetValue(version, out var factory))
        {
            throw new NotSupportedException(
                $"OpenAPI version {version} is not supported. " +
                $"Supported versions: {Factories.Aggregate("", (s, pair) => s + $", {pair.Key}").TrimStart(',', ' ')}");
        }

        return factory(parameterSpecification);
    }

    /// <summary>
    /// Create a parameter value parser for the specified OpenAPI version
    /// </summary>
    /// <param name="version">OpenAPI version (e.g., "2", "2.0", "3.1", "3.1.0" etc)</param>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON string</param>
    /// <returns>Parameter value parser</returns>
    /// <exception cref="NotSupportedException">The specified version is not supported</exception>
    public static IParameterValueParser OpenApi(string version, string parameterSpecificationAsJson)
    {
        var json = JsonNode.Parse(parameterSpecificationAsJson)?.AsObject() ??
                   throw new InvalidOperationException("Parameter specification is not a json object");
        return OpenApi(version, json);
    }

    /// <summary>
    /// Create a parameter value parser for the specified OpenAPI parameter
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    /// <exception cref="NotSupportedException">The specified parameter is not supported</exception>
    public static IParameterValueParser OpenApi(IParameter parameter) =>
        parameter switch
        {
            OpenApi20.Parameter p => p.CreateParameterValueParser(),
            global::OpenAPI.ParameterStyleParsers.OpenApi30.Parameter p => p.CreateParameterValueParser(),
            global::OpenAPI.ParameterStyleParsers.OpenApi31.Parameter p => p.CreateParameterValueParser(),
            global::OpenAPI.ParameterStyleParsers.OpenApi32.Parameter p => p.CreateParameterValueParser(),
            _ => throw new NotSupportedException(
                $"Parameter {parameter} is not supported.")
        };

    /// <summary>
    /// Create a parameter value parser for the specified OpenAPI parameter
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    /// <exception cref="NotSupportedException">The specified parameter is not supported</exception>
    public static IParameterValueParser CreateParameterValueParser(this IParameter parameter) =>
        OpenApi(parameter);
    
    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
#pragma warning disable CS0618 // Will be replaced by OpenAPI.ParameterStyleParsers.OpenApi31.ParameterValueParser
    public static ParameterParsers.ParameterValueParser OpenApi31(JsonObject parameterSpecification) => 
        ParameterParsers.ParameterValueParser.FromOpenApi31ParameterSpecification(parameterSpecification);
#pragma warning restore CS0618 

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
#pragma warning disable CS0618 // Will be replaced by OpenAPI.ParameterStyleParsers.OpenApi31.ParameterValueParser
    public static ParameterParsers.ParameterValueParser OpenApi31(string parameterSpecificationAsJson)
    {
        var json = JsonNode.Parse(parameterSpecificationAsJson)?.AsObject() ??
                   throw new InvalidOperationException("Parameter specification is not a json object");
        return ParameterParsers.ParameterValueParser.FromOpenApi31ParameterSpecification(json);
    }
#pragma warning restore CS0618

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi31.ParameterValueParser OpenApi31(OpenApi31.Parameter parameter) =>
        ParameterStyleParsers.OpenApi31.ParameterValueParser.Create(parameter);
    
    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.1
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi31.ParameterValueParser CreateParameterValueParser(this OpenApi31.Parameter parameter) =>
        ParameterStyleParsers.OpenApi31.ParameterValueParser.Create(parameter);

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.0
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi30.ParameterValueParser OpenApi30(JsonObject parameterSpecification) =>
        ParameterStyleParsers.OpenApi30.ParameterValueParser.Create(ParameterStyleParsers.OpenApi30.Parameter.FromOpenApi30ParameterSpecification(parameterSpecification));

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.0
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi30.ParameterValueParser OpenApi30(string parameterSpecificationAsJson) =>
        ParameterStyleParsers.OpenApi30.ParameterValueParser.Create(ParameterStyleParsers.OpenApi30.Parameter.FromOpenApi30ParameterSpecification(parameterSpecificationAsJson));

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.0
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi30.ParameterValueParser OpenApi30(OpenApi30.Parameter parameter) =>
        ParameterStyleParsers.OpenApi30.ParameterValueParser.Create(parameter);
    
    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.0
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi30.ParameterValueParser CreateParameterValueParser(this OpenApi30.Parameter parameter) =>
        ParameterStyleParsers.OpenApi30.ParameterValueParser.Create(parameter);

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.2
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi32.ParameterParsers.ParameterValueParser OpenApi32(JsonObject parameterSpecification) =>
        ParameterStyleParsers.OpenApi32.ParameterParsers.ParameterValueParser.Create(ParameterStyleParsers.OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterSpecification));

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.2
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi32.ParameterParsers.ParameterValueParser OpenApi32(string parameterSpecificationAsJson) =>
        ParameterStyleParsers.OpenApi32.ParameterParsers.ParameterValueParser.Create(ParameterStyleParsers.OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterSpecificationAsJson));

    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.2
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi32.ParameterParsers.ParameterValueParser OpenApi32(OpenApi32.Parameter parameter) =>
        ParameterStyleParsers.OpenApi32.ParameterParsers.ParameterValueParser.Create(parameter);
    
    /// <summary>
    /// Create a parameter value parser for OpenAPI 3.2
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi32.ParameterParsers.ParameterValueParser CreateParameterValueParser(this OpenApi32.Parameter parameter) =>
        ParameterStyleParsers.OpenApi32.ParameterParsers.ParameterValueParser.Create(parameter);

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
    
    /// <summary>
    /// Create a parameter value parser for OpenAPI 2.0
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi20.ParameterParsers.ParameterValueParser OpenApi20(OpenApi20.Parameter parameter) =>
        ParameterStyleParsers.OpenApi20.ParameterParsers.ParameterValueParser.Create(parameter);
    
    /// <summary>
    /// Create a parameter value parser for OpenAPI 2.0
    /// </summary>
    /// <param name="parameter">Parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static OpenApi20.ParameterParsers.ParameterValueParser CreateParameterValueParser(this OpenApi20.Parameter parameter) =>
        ParameterStyleParsers.OpenApi20.ParameterParsers.ParameterValueParser.Create(parameter);
}