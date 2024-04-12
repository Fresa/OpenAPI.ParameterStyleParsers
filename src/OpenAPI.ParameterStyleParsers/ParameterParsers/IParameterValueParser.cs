using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

public interface IParameterValueParser 
{
    bool TryParse(
        string value,
        out JsonNode? instance,
        [NotNullWhen(false)] out string? mappingError);

    /// <summary>
    /// Serializes a json node according to the specified parameter.
    /// It's assumed that the instance is valid according to the parameter's schema.
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    string? Serialize(JsonNode? instance);
}