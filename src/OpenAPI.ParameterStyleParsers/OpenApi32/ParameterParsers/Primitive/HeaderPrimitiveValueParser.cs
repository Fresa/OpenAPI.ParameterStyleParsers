using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Primitive;

internal sealed class HeaderPrimitiveValueParser : IValueParser
{
    private readonly InstanceType _type;

    internal HeaderPrimitiveValueParser(Parameter parameter)
    {
        var type = parameter.JsonSchema.GetInstanceType() ?? InstanceType.String;

        switch (type)
        {
            case InstanceType.Object:
            case InstanceType.Array:
                throw new ArgumentException(nameof(type),
                    $"Type '{Enum.GetName(type)}' is not a primitive type");
        }
        _type = type;
    }

    public bool TryParse(string? value, out JsonNode? instance, [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            instance = null;
            error = null;
            return true;
        }

        // Header: no percent-decoding
        return PrimitiveJsonConverter.TryConvert(value, _type, out instance, out error);
    }

    public string? Serialize(JsonNode? instance)
    {
        // Header: no percent-encoding
        return instance?.ToString();
    }
}