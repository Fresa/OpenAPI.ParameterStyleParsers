using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

internal abstract class PrimitiveValueParser : IValueParser
{
    public SchemaValueType Type { get; }
    internal bool Explode { get; }

    protected PrimitiveValueParser(bool explode, SchemaValueType type)
    {
        switch (type)
        {
            case SchemaValueType.Object:
            case SchemaValueType.Array:
                throw new ArgumentException(nameof(type),
                    $"Type '{Enum.GetName(type)}' is not a primitive type");
        }
        Explode = explode;
        Type = type;
    }

    internal static PrimitiveValueParser Create(Parameter parameter, JsonSchema jsonSchema)
    {
        var jsonType = jsonSchema.GetJsonType() ??
                       throw new ArgumentException("Missing 'type' attribute for schema");

        return parameter.Style switch
        {
            Parameter.Styles.Matrix => new MatrixPrimitiveValueParser(parameter.Explode, jsonType),
            Parameter.Styles.Simple => new SimplePrimitiveValueParser(parameter.Explode, jsonType),
            Parameter.Styles.Label => new LabelPrimitiveValueParser(parameter.Explode, jsonType),
            Parameter.Styles.Form => new FormPrimitiveValueParser(parameter.Explode, jsonType),
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
}