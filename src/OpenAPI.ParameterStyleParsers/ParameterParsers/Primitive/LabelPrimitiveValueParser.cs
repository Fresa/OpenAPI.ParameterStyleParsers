using System.Diagnostics.CodeAnalysis;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal sealed class LabelPrimitiveValueParser(Parameter parameter) : PrimitiveValueParser(parameter)
{
    protected override bool TryParse(
        string? input,
        out string? value,
        [NotNullWhen(false)] out string? error)
    {
        error = null;
        value = input?.TrimStart('.');
        return true;
    }
}