using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

public interface IParameterValueParser 
{
    bool TryParse(
        string[] values,
        out JsonNode? instance,
        [NotNullWhen(false)] out string? mappingError);
}