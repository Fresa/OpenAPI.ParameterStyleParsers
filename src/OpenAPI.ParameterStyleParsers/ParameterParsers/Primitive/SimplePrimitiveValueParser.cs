using System.Diagnostics.CodeAnalysis;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal sealed class SimplePrimitiveValueParser : PrimitiveValueParser
{
    public SimplePrimitiveValueParser(bool explode, SchemaValueType type) : base(explode, type)
    {
    }

    protected override bool TryParse(
        string input,
        out string? value,
        [NotNullWhen(false)] out string? error)
    {
        error = null;
        value = input;
        return true;
    }
}