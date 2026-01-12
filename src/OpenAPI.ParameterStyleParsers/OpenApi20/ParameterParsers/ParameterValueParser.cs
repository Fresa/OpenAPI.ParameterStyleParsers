using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers;

/// <summary>
/// Represents a parameter value parser for OpenAPI 2.0 styles
/// </summary>
[PublicAPI]
public sealed class ParameterValueParser : IParameterValueParser
{
    private readonly IValueParser _valueParser;

    private ParameterValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
        ValueIncludesParameterName = valueParser.ValueIncludesParameterName;
    }

    /// <summary>
    /// Creates a parameter value parser corresponding to the specified parameter
    /// </summary>
    /// <param name="parameter">The parameter specification</param>
    /// <returns>Parameter value parser</returns>
    public static ParameterValueParser Create(Parameter parameter)
    {
        var valueParser = CreateValueParser(parameter);
        return new ParameterValueParser(valueParser);
    }

    private static IValueParser CreateValueParser(Parameter parameter)
    {
        var jsonType = parameter.Type;

        return jsonType switch
        {
            Parameter.Types.String or
                Parameter.Types.Boolean or
                Parameter.Types.Integer or
                Parameter.Types.Number => PrimitiveValueParser.Create(parameter),
            Parameter.Types.Array => ArrayValueParser.Create(parameter),
            _ => throw new NotSupportedException($"Json type {jsonType} is not supported")
        };
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out JsonNode? instance,
        [NotNullWhen(false)] out string? error) =>
        _valueParser.TryParse(value, out instance, out error);

    /// <inheritdoc />
    public string? Serialize(JsonNode? instance) => 
        _valueParser.Serialize(instance);

    /// <inheritdoc />
    public bool ValueIncludesParameterName { get; }
}