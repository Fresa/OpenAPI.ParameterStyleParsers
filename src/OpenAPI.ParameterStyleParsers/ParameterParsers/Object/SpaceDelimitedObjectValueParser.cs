using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class SpaceDelimitedObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
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
            .Split("%20");
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> values) =>
        $";{(Explode ? "" : $"{parameter.Name}=")}{string.Join(Explode ? "&" : "%20",
            values.Select(pair =>
                $"{pair.Key}{(Explode ? "=" : "%20")}{pair.Value}"))}";
}