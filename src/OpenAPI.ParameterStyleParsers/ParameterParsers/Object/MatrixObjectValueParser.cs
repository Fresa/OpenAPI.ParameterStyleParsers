using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class MatrixObjectValueParser(Parameter parameter) : ObjectValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        var keyAndValues = value?
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .SelectMany(expression =>
            {
                var valueAndKey = expression.Split('=');
                var key = valueAndKey[0];
                var value = valueAndKey.Length == 1 ? string.Empty : valueAndKey.Last();
                return Explode ? [key, value] : value.Split(',');
            })
            .ToArray();

        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }

    protected override string Serialize(IDictionary<string, string?> properties) =>
        $";{(Explode ? "" : $"{ParameterName}=")}{string.Join(Explode ? ';' : ',',
            properties.Select(property => 
                $"{property.Key}{(Explode ? string.IsNullOrEmpty(property.Value) ? "" : "=" : ",")}{property.Value}"))}";
}