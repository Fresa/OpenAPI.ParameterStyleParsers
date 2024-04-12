using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal abstract class PrimitiveValueParser : IValueParser
{
    private SchemaValueType Type { get; }
    
    protected PrimitiveValueParser(Parameter parameter)
    {
        var type = parameter.JsonSchema.GetJsonType() ?? SchemaValueType.String;

        switch (type)
        {
            case SchemaValueType.Object:
            case SchemaValueType.Array:
                throw new ArgumentException(nameof(type),
                    $"Type '{Enum.GetName(type)}' is not a primitive type");
        }
        Type = type;
    }

    internal static PrimitiveValueParser Create(Parameter parameter)
    {
        return parameter.Style switch
        {
            Parameter.Styles.Matrix => new MatrixPrimitiveValueParser(parameter),
            Parameter.Styles.Simple => new SimplePrimitiveValueParser(parameter),
            Parameter.Styles.Label => new LabelPrimitiveValueParser(parameter),
            Parameter.Styles.Form => new FormPrimitiveValueParser(parameter),
            _ => throw new ArgumentException(nameof(parameter.Style),
                $"Style '{parameter.Style}' does not support primitive types")
        };
    }

    protected abstract bool TryParse(
        string? input,
        out string? value,
        [NotNullWhen(false)] out string? error);

    public bool TryParse(string? value, out JsonNode? instance,
        [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            instance = null;
            error = null;
            return true;
        }

        if (TryParse(value, out string? parsedValue, out error))
        {
            var unescapedValue = parsedValue == null ? null : Uri.UnescapeDataString(parsedValue);
            return PrimitiveJsonConverter.TryConvert(unescapedValue, Type, out instance, out error);
        }

        instance = null;
        return false;
    }

    public abstract string? Serialize(JsonNode? instance);
}