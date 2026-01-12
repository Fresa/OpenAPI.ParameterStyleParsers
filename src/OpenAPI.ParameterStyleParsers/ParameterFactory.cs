using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace OpenAPI.ParameterStyleParsers;

/// <summary>
/// Parameter factory
/// </summary>
[PublicAPI]
public static class ParameterFactory
{
    private static readonly Dictionary<string, Func<JsonObject, IParameter>> Factories = new()
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
    /// Create a parameter for the specified OpenAPI version
    /// </summary>
    /// <param name="version">OpenAPI version (e.g., "2", "2.0", "3.1", "3.1.0" etc)</param>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter</returns>
    /// <exception cref="NotSupportedException">The specified version is not supported</exception>
    public static IParameter OpenApi(string version, JsonObject parameterSpecification)
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
    /// Create a parameter for the specified OpenAPI version
    /// </summary>
    /// <param name="version">OpenAPI version (e.g., "2", "2.0", "3.1", "3.1.0" etc)</param>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON string</param>
    /// <returns>Parameter</returns>
    /// <exception cref="NotSupportedException">The specified version is not supported</exception>
    public static IParameter OpenApi(string version, string parameterSpecificationAsJson)
    {
        var json = JsonNode.Parse(parameterSpecificationAsJson)?.AsObject() ??
                   throw new InvalidOperationException("Parameter specification is not a json object");
        return OpenApi(version, json);
    }

    /// <summary>
    /// Create a parameter for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter</returns>
    public static OpenApi31.Parameter OpenApi31(JsonObject parameterSpecification) =>
        ParameterStyleParsers.OpenApi31.Parameter.FromOpenApi31ParameterSpecification(parameterSpecification);

    /// <summary>
    /// Create a parameter for OpenAPI 3.1
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON string</param>
    /// <returns>Parameter</returns>
    public static OpenApi31.Parameter OpenApi31(string parameterSpecificationAsJson) =>
        ParameterStyleParsers.OpenApi31.Parameter.FromOpenApi31ParameterSpecification(parameterSpecificationAsJson);

    /// <summary>
    /// Create a parameter for OpenAPI 3.0
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter</returns>
    public static OpenApi30.Parameter OpenApi30(JsonObject parameterSpecification) =>
        ParameterStyleParsers.OpenApi30.Parameter.FromOpenApi30ParameterSpecification(parameterSpecification);

    /// <summary>
    /// Create a parameter for OpenAPI 3.0
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON string</param>
    /// <returns>Parameter</returns>
    public static OpenApi30.Parameter OpenApi30(string parameterSpecificationAsJson) =>
        ParameterStyleParsers.OpenApi30.Parameter.FromOpenApi30ParameterSpecification(parameterSpecificationAsJson);

    /// <summary>
    /// Create a parameter for OpenAPI 3.2
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter</returns>
    public static OpenApi32.Parameter OpenApi32(JsonObject parameterSpecification) =>
        ParameterStyleParsers.OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterSpecification);

    /// <summary>
    /// Create a parameter for OpenAPI 3.2
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON string</param>
    /// <returns>Parameter</returns>
    public static OpenApi32.Parameter OpenApi32(string parameterSpecificationAsJson) =>
        ParameterStyleParsers.OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterSpecificationAsJson);

    /// <summary>
    /// Create a parameter for OpenAPI 2.0
    /// </summary>
    /// <param name="parameterSpecification">Parameter specification as JSON</param>
    /// <returns>Parameter</returns>
    public static OpenApi20.Parameter OpenApi20(JsonObject parameterSpecification) =>
        ParameterStyleParsers.OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterSpecification);

    /// <summary>
    /// Create a parameter for OpenAPI 2.0
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Parameter specification as JSON string</param>
    /// <returns>Parameter</returns>
    public static OpenApi20.Parameter OpenApi20(string parameterSpecificationAsJson) =>
        ParameterStyleParsers.OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterSpecificationAsJson);
}