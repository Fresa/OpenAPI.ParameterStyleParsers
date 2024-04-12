using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class SimpleObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        var keyAndValues = value?.Split(',');
        if (Explode)
        {
            keyAndValues = keyAndValues?
                .SelectMany(value => value
                    .Split('='))
                .ToArray();
        }
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> values) => 
        string.Join(',', 
            values.Select(pair => $"{pair.Key}{(Explode ? "=" : ",")}{pair.Value}"));
}