using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Object;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

internal sealed class MissingSchemaTypeValueParser : IValueParser
{
    private readonly IValueParser _valueParser;

    private MissingSchemaTypeValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
    }

    internal static MissingSchemaTypeValueParser Create(Parameter parameter)
    {
        if (parameter.Style == Parameter.Styles.DeepObject)
            return new MissingSchemaTypeValueParser(new DeepObjectValueParser(parameter));
        var arrayValueParser = ArrayValueParser.Create(parameter);
        return new MissingSchemaTypeValueParser(arrayValueParser);
    }

    public bool TryParse(string? value, out JsonNode? instance, 
        [NotNullWhen(false)] out string? error)
    {
        return _valueParser.TryParse(value, out instance, out error);
    }
}