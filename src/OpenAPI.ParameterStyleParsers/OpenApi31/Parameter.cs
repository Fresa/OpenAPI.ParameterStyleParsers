using OpenAPI.ParameterStyleParsers.JsonSchema;

namespace OpenAPI.ParameterStyleParsers.OpenApi31;

/// <summary>
/// An OpenAPI 3.1 parameter specification
/// </summary>
public record Parameter
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

    private Parameter(string name, string style, bool explode, IJsonSchema jsonSchema)
    {
        Name = name;
        Style = style;
        Explode = explode;
        JsonSchema = jsonSchema;
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

        return new Parameter(name, style, explode, jsonSchema);
    }

    /// <summary>
    /// The name of the parameter
    /// </summary>
    public string Name { get; private init; }
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