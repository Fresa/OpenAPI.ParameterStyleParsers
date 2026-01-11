using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using JetBrains.Annotations;

namespace OpenAPI.ParameterStyleParsers;

/// <summary>
/// Common interface for parameter value parsers across OpenAPI versions
/// </summary>
[PublicAPI]
public interface IParameterValueParser
{
    /// <summary>
    /// Parses a style formatted parameter value to a json node.
    /// It's assumed that the input is valid according to the style format.
    /// </summary>
    /// <param name="value">Style formatted input</param>
    /// <param name="instance">The parsed json if this method returns true</param>
    /// <param name="error">Parsing error if this method returns false</param>
    /// <returns>true if an instance could be constructed, false if there are errors</returns>
    bool TryParse(string? value, out JsonNode? instance, [NotNullWhen(false)] out string? error);

    /// <summary>
    /// Serializes a json node according to the specified parameter.
    /// It's assumed that the instance is valid according to the parameter's schema.
    /// </summary>
    /// <param name="instance">Json instance</param>
    /// <returns>Style formatted instance</returns>
    string? Serialize(JsonNode? instance);
    
    /// <summary>
    /// Parameter name is included in the value, i.e. [parameter name]=[value] vs [value], see style examples in OAS.
    /// </summary>
    bool ValueIncludesParameterName { get; }
}