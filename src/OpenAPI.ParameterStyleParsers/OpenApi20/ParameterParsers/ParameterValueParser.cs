using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.Json;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers;

/// <summary>
/// Represents a parameter value parser for OpenAPI styles
/// </summary>
public sealed class ParameterValueParser
{
    private readonly IValueParser _valueParser;

    private ParameterValueParser(IValueParser valueParser)
    {
        _valueParser = valueParser;
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

    /// <summary>
    /// Create a parameter value parser from an OpenAPI 2.0 parameter specification
    /// <see href="https://spec.openapis.org/oas/v2.0#parameter-object"/>
    /// </summary>
    /// <param name="parameterSpecification">Specification of the parameter</param>
    /// <returns>Parameter value parser, or null if In = Body</returns>
    /// <exception cref="InvalidOperationException">The provided json object doesn't correspond to the specification</exception>
    public static ParameterValueParser? FromOpenApi20ParameterSpecification(JsonObject parameterSpecification)
    {
        var name = parameterSpecification.GetRequiredPropertyValue<string>(Parameter.FieldNames.Name);
        if (name == string.Empty)
            throw new InvalidOperationException($"Property '{Parameter.FieldNames.Name}' is empty string");

        var location = parameterSpecification.GetRequiredPropertyValue<string>(Parameter.FieldNames.In);
        if (!Parameter.Locations.All.Contains(location))
        {
            throw new InvalidOperationException(
                $"Property '{Parameter.FieldNames.In}' has an invalid value '{location}'. Expected any of {string.Join(", ", Parameter.Locations.All)}");
        }

        if (location == Parameter.Locations.Body)
        {
            return null;
        }
        
        parameterSpecification.TryGetPropertyValue(Parameter.FieldNames.CollectionFormat, out var collectionFormatJson);
        var collectionFormat = collectionFormatJson?.GetValue<string>() switch
        {
            null => Parameter.CollectionFormats.Csv,
            var value when Parameter.CollectionFormats.All.Contains(value) => value!,
            var value => throw new InvalidOperationException(
                $"Property '{Parameter.FieldNames.CollectionFormat}' has an invalid value '{value}'. Expected any of {string.Join(", ", Parameter.CollectionFormats.All)}")
        };

        var type = parameterSpecification.GetRequiredPropertyValue<string>(Parameter.FieldNames.Type);
        if (!Parameter.Types.All.Contains(type))
        {
            throw new InvalidOperationException(
                $"Property '{Parameter.FieldNames.Type}' has an invalid value '{type}'. Expected any of {string.Join(", ", Parameter.Types.All)}");
        }

        ItemsObject? items = null;
        if (type == Parameter.Types.Array)
        {
            if (parameterSpecification.GetRequiredPropertyValue(Parameter.FieldNames.Items) is not JsonObject itemType)
            {
                throw new InvalidOperationException(
                    $"Property '{Parameter.FieldNames.Items}' has no value or is not an object");
            }

            items = ItemsObject.FromOpenApi20ItemsObjectSpecification(itemType);
        }
        var parameter = Parameter.Parse(
            name, 
            location, 
            type: type,
            collectionFormat: collectionFormat,
            items: items);
        return Create(parameter);
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

    /// <summary>
    /// Parses a style formatted parameter value to a json node.
    /// It's assumed that the input is valid according to the style format.
    /// </summary>
    /// <param name="value">Style formatted input</param>
    /// <param name="instance">The parsed json if this method returns true</param>
    /// <param name="error">Parsing error if this method returns false</param>
    /// <returns>true if an instance could be constructed, false if there are errors</returns>
    public bool TryParse(string? value, out JsonNode? instance,
        [NotNullWhen(false)] out string? error) =>
        _valueParser.TryParse(value, out instance, out error);

    /// <summary>
    /// Serializes a json node according to the specified parameter.
    /// It's assumed that the instance is valid according to the parameter's schema.
    /// </summary>
    /// <param name="instance">Json instance</param>
    /// <returns>Style formatted instance</returns>
    public string? Serialize(JsonNode? instance) => 
        _valueParser.Serialize(instance);
}