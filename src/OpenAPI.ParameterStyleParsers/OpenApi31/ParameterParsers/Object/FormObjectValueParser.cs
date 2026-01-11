using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Object;

internal sealed class FormObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    internal override bool ValueIncludesParameterName => true;

    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (Explode)
        {
            error = "form style with explode not supported for objects as the parameter name cannot be determined";
            obj = null;
            return false;
        }

        var keyAndValues = value?
            .Split('=')
            .Last()
            .Split(',');
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> properties) =>
        $";{ParameterName}={string.Join(',', properties.Select(pair => $"{pair.Key},{pair.Value}"))}";
}