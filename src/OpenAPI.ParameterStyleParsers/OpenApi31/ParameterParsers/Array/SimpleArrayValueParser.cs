using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Array;

internal sealed class SimpleArrayValueParser(Parameter parameter) : ArrayValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var arrayValues = value?
            .Split(',');
        return TryGetArrayItems(arrayValues, out array, out error);
    }

    public override bool ValueIncludesParameterName => false;

    protected override string Serialize(string?[] values) => 
        string.Join(',', values);
}