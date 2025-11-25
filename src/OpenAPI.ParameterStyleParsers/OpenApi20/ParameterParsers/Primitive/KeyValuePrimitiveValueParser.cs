using System.Diagnostics.CodeAnalysis;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

internal sealed class KeyValuePrimitiveValueParser(Parameter parameter) : PrimitiveValueParser(parameter)
{
    protected override bool TryParse(
        string input,
        [NotNullWhen(true)] out string? value,
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