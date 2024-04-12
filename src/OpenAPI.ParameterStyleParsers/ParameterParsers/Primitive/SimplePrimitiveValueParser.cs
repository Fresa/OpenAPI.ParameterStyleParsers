using System.Diagnostics.CodeAnalysis;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal sealed class SimplePrimitiveValueParser(Parameter parameter) : PrimitiveValueParser(parameter)
{
    protected override bool TryParse(
        string? input,
        out string? value,
        [NotNullWhen(false)] out string? error)
    {
        error = null;
        value = input;
        return true;
    }

    protected override string Serialize(string value) =>
        value;
}