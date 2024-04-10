using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

internal sealed class DeepObjectValueParser(bool explode, JsonSchema schema) : ObjectValueParser(schema, explode)
{
    public override bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (!Explode)
        {
            error = "deep object style without explode is not supported for objects";
            obj = null;
            return false;
        }

        var keyAndValues = value?
            .Split('&')
            .SelectMany(value =>
            {
                var keyAndValue = value
                    .Split('=');
                var key = keyAndValue.First();
                return new[]
                {
                    key[(key.IndexOf('[') + 1)..key.IndexOf(']')],
                    keyAndValue.Last()
                };
            })
            .ToArray();
        return TryGetObjectProperties(keyAndValues, out obj, out error);
    }
}