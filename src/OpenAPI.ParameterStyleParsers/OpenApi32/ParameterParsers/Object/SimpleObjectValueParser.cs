using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Object;

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
                .SelectMany(v => v.Split('='))
                .ToArray();
        }
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> properties) =>
        string.Join(',', properties.Select(pair => $"{pair.Key}{(Explode ? "=" : ",")}{pair.Value}"));
}