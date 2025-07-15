using System.Diagnostics.CodeAnalysis;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

internal sealed class ValuePrimitiveValueParser(Parameter parameter) : PrimitiveValueParser(parameter)
{
    protected override bool TryParse(
        string input,
        [NotNullWhen(true)] out string? value,
        [NotNullWhen(false)] out string? error)
    {
        error = null;
        value = input;
        return true;
    }

    protected override string Serialize(string value) =>
        value;
}