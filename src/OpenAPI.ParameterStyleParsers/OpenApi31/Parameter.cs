using System.Text.Json.Nodes;
using JetBrains.Annotations;
using OpenAPI.ParameterStyleParsers.Json;
using OpenAPI.ParameterStyleParsers.JsonSchema;

namespace OpenAPI.ParameterStyleParsers.OpenApi31;

/// <summary>
/// An OpenAPI 3.1 parameter specification
/// </summary>
[PublicAPI]
public record Parameter : IParameter
{
    /// <summary>
    /// Supported OpenAPI parameter styles
    /// </summary>
    public static class Styles
    {
#pragma warning disable CS1591
        public const string Matrix = "matrix";
        public const string Label = "label";
        public const string Simple = "simple";
        public const string Form = "form";
        public const string SpaceDelimited = "spaceDelimited";
        public const string PipeDelimited = "pipeDelimited";
        public const string DeepObject = "deepObject";
        public static readonly string[] All = [Matrix, Label, Simple, Form, SpaceDelimited, PipeDelimited, DeepObject];
#pragma warning restore CS1591
    }

    /// <summary>
    /// Supported OpenAPI parameter locations
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global Part of public contract
    public static class Locations
    {
#pragma warning disable CS1591
        public const string Path = "path";
        public const string Header = "header";
        public const string Query = "query";
        public const string Cookie = "cookie";
        public static readonly string[] All = [Path, Header, Query, Cookie];
#pragma warning restore CS1591
    }

    /// <summary>
    /// A map between parameter locations and supported styles
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global Part of public contract
    public static readonly Dictionary<string, string[]> LocationToStylesMap = new()
    {
        [Locations.Path] = [Styles.Matrix, Styles.Label, Styles.Simple],
        [Locations.Header] = [Styles.Simple],
        [Locations.Query] = [Styles.Form, Styles.SpaceDelimited, Styles.PipeDelimited, Styles.DeepObject],
        [Locations.Cookie] = [Styles.Form],
    };

    /// <summary>
    /// Relevant field names for the parameter
    /// </summary>
    public static class FieldNames
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string Name = "name";
        public const string In = "in";
        public const string Style = "style";
        public const string Explode = "explode";
        public const string Schema = "schema";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    private Parameter(string name, string location, string style, bool explode, IJsonSchema jsonSchema)
    {
        Name = name;
        Style = style;
        Explode = explode;
        JsonSchema = jsonSchema;
        
        InPath = location == Locations.Path;
        InQuery = location == Locations.Query;
        InHeader = location == Locations.Header;
        InCookie = location == Locations.Cookie;
    }

    /// <summary>
    /// Parses an OpenAPI parameter
    /// </summary>
    /// <param name="name">Parameter name</param>
    /// <param name="style">Parameter style format</param>
    /// <param name="location">The location of the parameter</param>
    /// <param name="explode">If the parameter apply explode</param>
    /// <param name="jsonSchema">The parameters json schema</param>
    /// <returns>A parameter specification</returns>
    /// <exception cref="InvalidOperationException">Thrown if location and styles are incompatible</exception>
    public static Parameter Parse(string name, string style, string location, bool explode, IJsonSchema jsonSchema)
    {
        if (!LocationToStylesMap.TryGetValue(location, out var styles))
        {
            throw new InvalidOperationException(
                $"location '{location}' is not a valid location. Valid locations are {Locations.Cookie}, {Locations.Header}, {Locations.Query} and {Locations.Path}");
        }

        if (!styles.Contains(style))
        {
            throw new InvalidOperationException(
                $"location '{location}' does not support style '{style}'. Supported styles are {string.Join(", ", styles)}");
        }

        return new Parameter(name, location, style, explode, jsonSchema);
    }

    /// <summary>
    /// Create a parameter from an OpenAPI 3.1 parameter specification
    /// <see href="https://spec.openapis.org/oas/v3.1.0#parameter-object"/>
    /// </summary>
    /// <param name="parameterSpecificationAsJson">Specification of the parameter</param>
    /// <returns>Parameter</returns>
    /// <exception cref="InvalidOperationException">The provided json object doesn't correspond to the specification</exception>
    public static Parameter FromOpenApi31ParameterSpecification(string parameterSpecificationAsJson)
    {
        var json = JsonNode.Parse(parameterSpecificationAsJson)?.AsObject() ??
                   throw new InvalidOperationException("Parameter specification is not a json object");
        return FromOpenApi31ParameterSpecification(json);
    }

    /// <summary>
    /// Create a parameter from an OpenAPI 3.1 parameter specification
    /// <see href="https://spec.openapis.org/oas/v3.1.0#parameter-object"/>
    /// </summary>
    /// <param name="parameterSpecification">Specification of the parameter</param>
    /// <returns>The parsed parameter</returns>
    /// <exception cref="InvalidOperationException">The provided json object doesn't correspond to the specification</exception>
    public static Parameter FromOpenApi31ParameterSpecification(JsonObject parameterSpecification)
    {
        var name = parameterSpecification.GetRequiredPropertyValue<string>(FieldNames.Name);
        if (name == string.Empty)
            throw new InvalidOperationException($"Property '{FieldNames.Name}' is empty string");

        var location = parameterSpecification.GetRequiredPropertyValue<string>(FieldNames.In);
        if (!Locations.All.Contains(location))
        {
            throw new InvalidOperationException(
                $"Property '{FieldNames.In}' has an invalid value '{location}'. Expected any of {string.Join(", ", Locations.All)}");
        }

        string style;
        if (parameterSpecification.TryGetPropertyValue(FieldNames.Style, out var styleJson))
        {
            style = styleJson?.GetValue<string>() switch
            {
                var value when Styles.All.Contains(value) => value!,
                var value => throw new InvalidOperationException(
                    $"Property '{FieldNames.Style}' has an invalid value '{value}'. Expected any of {string.Join(", ", Styles.All)}")
            };
        }
        else
        {
            style = location switch
            {
                Locations.Path => Styles.Simple,
                Locations.Cookie => Styles.Form,
                Locations.Query => Styles.Form,
                Locations.Header => Styles.Simple,
                _ => throw new InvalidOperationException($"Unknown location {location}")
            };
        }

        parameterSpecification.TryGetPropertyValue(FieldNames.Explode, out var explodeJson);
        var explode = explodeJson?.GetValue<bool>() ?? style == Styles.Form;

        var schemaJson = parameterSpecification.GetRequiredPropertyValue(FieldNames.Schema);
        var schema = new JsonSchema202012(schemaJson);

        return Parse(name, style, location, explode, schema);
    }

    /// <inheritdoc />
    public string Name { get; private init; }

    /// <inheritdoc />
    public bool InBody => false;
    
    /// <inheritdoc />
    public bool InHeader { get; private init; }
    
    /// <inheritdoc />
    public bool InPath { get; private init; }
    
    /// <inheritdoc />
    public bool InQuery { get; private init; }

    /// <inheritdoc />
    public bool InFormData => false;
    
    /// <inheritdoc />
    public bool InCookie { get; private init; }
    
    /// <summary>
    /// The style of the parameter
    /// </summary>
    public string Style { get; private init; }
    
    /// <summary>
    /// If the parameter apply explosion of the serialized format
    /// </summary>
    public bool Explode { get; private init; }
    
    /// <summary>
    /// The parameter's json schema
    /// </summary>
    public IJsonSchema JsonSchema { get; private init; }
}