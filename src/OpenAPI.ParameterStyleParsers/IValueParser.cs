using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers;

internal interface IValueParser
{
    internal bool TryParse(
        string? value,
        out JsonNode? instance,
        [NotNullWhen(false)] out string? error);

    string? Serialize(JsonNode? instance);
    
    bool ValueIncludesParameterName { get; }
}