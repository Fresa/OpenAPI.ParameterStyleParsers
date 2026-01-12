using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers.Primitive;

internal abstract class PrimitiveValueParser(Parameter parameter) : IValueParser
{
    private InstanceType Type { get; } = GetInstanceType(parameter);
    protected string ParameterName { get; } = parameter.Name;

    private static InstanceType GetInstanceType(Parameter parameter)
    {
        var type = parameter.JsonSchema.GetInstanceType() ?? InstanceType.String;
        return type switch
        {
            InstanceType.Object or InstanceType.Array =>
                throw new ArgumentException(nameof(type),
                    $"Type '{Enum.GetName(type)}' is not a primitive type"),
            _ => type
        };
    }

    protected abstract bool TryParse(
        string input,
        out string? value,
        [NotNullWhen(false)] out string? error);

    public bool TryParse(string? value, out JsonNode? instance, [NotNullWhen(false)] out string? error)
    {
        if (value == null)
        {
            instance = null;
            error = null;
            return true;
        }

        if (TryParse(value, out string? parsedValue, out error))
        {
            return PrimitiveJsonConverter.TryConvert(parsedValue, Type, out instance, out error);
        }

        instance = null;
        return false;
    }

    public string? Serialize(JsonNode? instance) =>
        instance == null ? null : Serialize(instance.ToString());

    public abstract bool ValueIncludesParameterName { get; }

    protected abstract string Serialize(string value);
}