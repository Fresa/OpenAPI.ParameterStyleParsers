using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi30;

/// <summary>
/// Represents a parameter value parser for OpenAPI 3.0 styles
/// </summary>
[PublicAPI]
public sealed class ParameterValueParser : IParameterValueParser
{
    private readonly OpenApi31.ParameterValueParser _innerParser;

    private ParameterValueParser(OpenApi31.ParameterValueParser innerParser)
    {
        _innerParser = innerParser;
    }

    /// <inheritdoc />
    public bool ValueIncludesParameterName => _innerParser.ValueIncludesParameterName;

    /// <summary>
    /// Creates a parameter value parser corresponding to the specified parameter
    /// </summary>
    /// <param name="parameter">The parameter specification</param>
    /// <returns>Parameter value parser</returns>
    [PublicAPI]
    public static ParameterValueParser Create(Parameter parameter)
    {
        // OpenAPI 3.0 and 3.1 have identical parameter serialization rules,
        // so we delegate to the 3.1 parser
        var openApi31Parameter = OpenApi31.Parameter.Parse(
            parameter.Name,
            parameter.Style,
            parameter.Location,
            parameter.Explode,
            parameter.JsonSchema);

        var innerParser = OpenApi31.ParameterValueParser.Create(openApi31Parameter);
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
    [PublicAPI]
    public bool TryParse(string? value, out JsonNode? instance,
        [NotNullWhen(false)] out string? error) =>
        _innerParser.TryParse(value, out instance, out error);

    /// <summary>
    /// Serializes a json node according to the specified parameter.
    /// It's assumed that the instance is valid according to the parameter's schema.
    /// </summary>
    /// <param name="instance">Json instance</param>
    /// <returns>Style formatted instance</returns>
    [PublicAPI]
    public string? Serialize(JsonNode? instance) =>
        _innerParser.Serialize(instance);
}