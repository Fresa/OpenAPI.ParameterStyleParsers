using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.Extensions;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Array;

internal sealed class SimpleArrayValueParser(Parameter parameter) : ArrayValueParser(parameter)
{
    public override bool TryParse(
        string? value,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var arrayValues = value?.SplitWithQuotation(',');
        return TryGetArrayItems(arrayValues, out array, out error);
    }

    public override bool ValueIncludesParameterName => false;

    protected override string Serialize(string?[] values) =>
        string.Join(',', values);
}