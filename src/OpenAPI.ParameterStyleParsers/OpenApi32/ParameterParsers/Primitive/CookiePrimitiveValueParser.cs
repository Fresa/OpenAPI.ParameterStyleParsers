using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Primitive;

internal sealed class CookiePrimitiveValueParser : IValueParser
{
    private readonly InstanceType _type;
    private readonly string _parameterName;

    internal CookiePrimitiveValueParser(Parameter parameter)
    {
        var type = parameter.JsonSchema.GetInstanceType() ?? InstanceType.String;
        _parameterName = parameter.Name;

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

        // Cookie style: no percent-encoding
        var parser = new InputParser(value);
        if (!parser.Expect(_parameterName, out error))
        {
            instance = null;
            return false;
        }

        if (!parser.Expect("=", out error))
        {
            instance = null;
            return false;
        }

        return PrimitiveJsonConverter.TryConvert(parser.Current, _type, out instance, out error);
    }

    public string? Serialize(JsonNode? instance)
    {
        // Cookie style: no percent-encoding
        return instance == null ? null : $"{_parameterName}={instance}";
    }
}