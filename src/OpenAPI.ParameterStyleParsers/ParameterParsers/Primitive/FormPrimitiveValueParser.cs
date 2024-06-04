using System.Diagnostics.CodeAnalysis;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal sealed class FormPrimitiveValueParser(Parameter parameter) : PrimitiveValueParser(parameter)
{
    protected override bool TryParse(
        string input,
        out string? value,
        [NotNullWhen(false)] out string? error)
    {
        var parser = new InputParser(input);
        if (!parser.Expect(ParameterName, out error))
        {
            value = null;
            return false;
        }

        if (!parser.Expect("=", out error))
        {
            value = null;
            return false;
        }

        value = parser.Current;
        return true;
    }

    protected override string Serialize(string value) =>
        $"{ParameterName}={value}";
}