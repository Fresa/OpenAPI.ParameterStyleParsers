using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Object;

internal sealed class PipeDelimitedObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    internal override bool ValueIncludesParameterName => false;

    public override bool TryParse(
        string? value,
        [NotNullWhen(true)] out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (Explode)
        {
            error = "pipe delimited style with explode not supported for objects as the parameter name cannot be determined";
            obj = null;
            return false;
        }

        var keyAndValues = value?
            .Split('=')
            .Last()    
            .Split('|');
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> properties) =>
        string.Join('|', properties.Select(property => $"{property.Key}|{property.Value}"));
}