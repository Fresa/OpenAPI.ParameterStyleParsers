using System.Diagnostics.CodeAnalysis;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Primitive;

internal sealed class SimplePrimitiveValueParser(Parameter parameter) : PrimitiveValueParser(parameter)
{
    protected override bool TryParse(string input, out string? value, [NotNullWhen(false)] out string? error)
    {
        value = input;
        error = null;
        return true;
    }

    public override bool ValueIncludesParameterName => false;
    protected override string Serialize(string value) => value;
}