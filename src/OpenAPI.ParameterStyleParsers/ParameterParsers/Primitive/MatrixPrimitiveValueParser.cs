using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal sealed class MatrixPrimitiveValueParser(Parameter parameter) : PrimitiveValueParser(parameter)
{
    protected override bool TryParse(
        string? input,
        out string? value,
        [NotNullWhen(false)] out string? error)
    {
        error = null;
        value = input?.IndexOf('=') > -1 ? input[(input.IndexOf('=') + 1)..] : string.Empty;
        return true;
    }

    public override string Serialize(JsonNode? instance)
    {
        throw new NotImplementedException();
    }
}