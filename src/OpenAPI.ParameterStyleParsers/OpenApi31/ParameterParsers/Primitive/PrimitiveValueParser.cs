using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;

internal abstract class PrimitiveValueParser : IValueParser
{
    private InstanceType Type { get; }
    
    protected string ParameterName { get; }

    protected PrimitiveValueParser(Parameter parameter)
    {
        var type = parameter.JsonSchema.GetInstanceType() ?? InstanceType.String;
        ParameterName = parameter.Name;

        switch (type)
        {
            case InstanceType.Object:
            case InstanceType.Array:
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
        string input,
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

        var unescapedValue = Uri.UnescapeDataString(value);
        if (TryParse(unescapedValue, out string? parsedValue, out error))
        {
            return PrimitiveJsonConverter.TryConvert(parsedValue, Type, out instance, out error);
        }

        instance = null;
        return false;
    }

    public string? Serialize(JsonNode? instance) =>
        instance == null ? null : Serialize(Uri.EscapeDataString(instance.ToString()));
    protected abstract string Serialize(string value);
}