using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.Json;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Object;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

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
    /// Create a parameter value parser from an OpenAPI 3.1 parameter specification
    /// <see href="https://spec.openapis.org/oas/v3.1.0#parameter-object"/>
    /// </summary>
    /// <param name="parameterSpecification">Specification of the parameter</param>
    /// <returns>Parameter value parser</returns>
    /// <exception cref="InvalidOperationException">The provided json object doesn't correspond to the specification</exception>
    public static ParameterValueParser FromOpenApi31ParameterSpecification(JsonObject parameterSpecification)
    {
        var name = parameterSpecification.GetRequiredPropertyValue<string>(Parameter.FieldNames.Name);
        if (name == string.Empty)
            throw new InvalidOperationException($"Property '{Parameter.FieldNames.Name}' is empty string");

        var location = parameterSpecification.GetRequiredPropertyValue<string>(Parameter.FieldNames.In);
        if (!Parameter.Locations.All.Contains(location))
        {
            throw new InvalidOperationException(
                $"Property 'in' has an invalid value '{location}'. Expected any of {string.Join(", ", Parameter.Locations.All)}");
        }

        string style;
        if (parameterSpecification.TryGetPropertyValue("style", out var styleJson))
        {
            style = styleJson?.GetValue<string>() switch
            {
                var value when Parameter.Styles.All.Contains(value) => value!,
                var value => throw new InvalidOperationException(
                    $"Property 'style' has an invalid value '{value}'. Expected any of {string.Join(", ", Parameter.Styles.All)}")
            };
        }
        else
        {
            style = location switch
            {
                Parameter.Locations.Path => Parameter.Styles.Simple,
                Parameter.Locations.Cookie => Parameter.Styles.Form,
                Parameter.Locations.Query => Parameter.Styles.Form,
                Parameter.Locations.Header => Parameter.Styles.Simple,
                _ => throw new InvalidOperationException($"Unknown location {location}")
            };
        }

        parameterSpecification.TryGetPropertyValue(Parameter.FieldNames.Explode, out var explodeJson);
        var explode = explodeJson?.GetValue<bool>() ?? style == Parameter.Styles.Form;

        var schemaJson = parameterSpecification.GetRequiredPropertyValue(Parameter.FieldNames.Schema);
        var schema = new JsonSchema202012(schemaJson);

        var parameter = Parameter.Parse(name, style, location, explode, schema);
        return Create(parameter);
    }

    private static IValueParser CreateValueParser(Parameter parameter)
    {
        var jsonSchema = parameter.JsonSchema;
        var jsonType = jsonSchema.GetInstanceType();

        return jsonType switch
        {
            null => MissingSchemaTypeValueParser.Create(parameter),
            InstanceType.String or
                InstanceType.Boolean or
                InstanceType.Integer or
                InstanceType.Number or
                InstanceType.Null => PrimitiveValueParser.Create(parameter),
            InstanceType.Array => ArrayValueParser.Create(parameter),
            InstanceType.Object => ObjectValueParser.Create(parameter),
            _ => throw new NotSupportedException($"Json type {Enum.GetName(jsonType.Value)} is not supported")
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