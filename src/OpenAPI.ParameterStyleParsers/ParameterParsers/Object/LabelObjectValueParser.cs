using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class LabelObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        var keyAndValues = value?
            .Split('.')[1..];
        if (Explode)
        {
            keyAndValues = keyAndValues?
                .SelectMany(keyAndValue => keyAndValue
                    .Split('='))
                .ToArray();
        }
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> properties)
    {
        if (!properties.Any())
            return string.Empty;
        return $".{string.Join('.',
            properties.Select(pair => $"{pair.Key}{(Explode ? "=" : ".")}{pair.Value}"))}";
    }
}