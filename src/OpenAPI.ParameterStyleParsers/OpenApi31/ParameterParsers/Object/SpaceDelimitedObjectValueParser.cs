using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Object;

internal sealed class SpaceDelimitedObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    public override bool ValueIncludesParameterName => true;
    public override string Delimiter => "%20";

    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (Explode)
        {
            error = "space delimited style with explode not supported for objects as the parameter name cannot be determined";
            obj = null;
            return false;
        }

        var keyAndValues = value?
            .Split('=')
            .Last()
            .Split(Delimiter);
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> properties) =>
        $"{ParameterName}={string.Join(Delimiter, properties.Select(property => $"{property.Key}{Delimiter}{property.Value}"))}";
}