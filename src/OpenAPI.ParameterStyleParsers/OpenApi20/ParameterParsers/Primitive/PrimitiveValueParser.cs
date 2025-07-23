using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

internal abstract class PrimitiveValueParser : IValueParser
{
    private string Type { get; }
    
    protected string ParameterName { get; }

    internal PrimitiveValueParser(Parameter parameter)
    {
        if (parameter.InBody || parameter.IsArray || !Parameter.Types.Primitives.Contains(parameter.Type))
        {
            throw new InvalidOperationException(
                $"Parameter '{parameter.Name}' does not declare a primitive type: {parameter.Type}");
        }

        ParameterName = parameter.Name;
        Type = parameter.Type;
    }

    internal static PrimitiveValueParser Create(Parameter parameter)
    {
        return parameter switch
        {
            _ when parameter.InFormData || parameter.InQuery => new KeyValuePrimitiveValueParser(parameter),
            _ when parameter.InPath || parameter.InHeader => new ValuePrimitiveValueParser(parameter),
            _ => throw new ArgumentException(nameof(parameter.Type),
                $"Parameter '{parameter.Name}' is not a primitive type")
        };
    }
    
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

    protected abstract bool TryParse(
        string input,
        [NotNullWhen(true)] out string? value,
        [NotNullWhen(false)] out string? error);

    public string? Serialize(JsonNode? instance) =>
        instance == null ? null : Serialize(Uri.EscapeDataString(instance.ToString()));

    protected abstract string Serialize(string value);
}