using OpenAPI.ParameterStyleParsers.JsonSchema;

namespace OpenAPI.ParameterStyleParsers;

/// <summary>
/// An OpenAPI parameter specification
/// </summary>
[Obsolete("Use OpenAPI.ParameterStyleParsers.OpenApi31.Parameter instead")]
public record Parameter
{
    /// <summary>
    /// Supported OpenAPI parameter styles
    /// </summary>
    public static class Styles
    {
#pragma warning disable CS1591
        public const string Matrix = OpenApi31.Parameter.Styles.Matrix;
        public const string Label = OpenApi31.Parameter.Styles.Label;
        public const string Simple = OpenApi31.Parameter.Styles.Simple;
        public const string Form = OpenApi31.Parameter.Styles.Form;
        public const string SpaceDelimited = OpenApi31.Parameter.Styles.SpaceDelimited;
        public const string PipeDelimited = OpenApi31.Parameter.Styles.PipeDelimited;
        public const string DeepObject = OpenApi31.Parameter.Styles.DeepObject;
        public static readonly string[] All = OpenApi31.Parameter.Styles.All;
#pragma warning restore CS1591
    }

    /// <summary>
    /// Supported OpenAPI parameter locations
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global Part of public contract
    public static class Locations
    {
#pragma warning disable CS1591
        public const string Path = OpenApi31.Parameter.Locations.Path;
        public const string Header = OpenApi31.Parameter.Locations.Header;
        public const string Query = OpenApi31.Parameter.Locations.Query;
        public const string Cookie = OpenApi31.Parameter.Locations.Cookie;
        public static readonly string[] All = OpenApi31.Parameter.Locations.All;
#pragma warning restore CS1591
    }

    /// <summary>
    /// A map between parameter locations and supported styles
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global Part of public contract
    public static readonly Dictionary<string, string[]> LocationToStylesMap =
        OpenApi31.Parameter.LocationToStylesMap;

    /// <summary>
    /// Relevant field names for the parameter
    /// </summary>
    public static class FieldNames
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string Name = OpenApi31.Parameter.FieldNames.Name;
        public const string In = OpenApi31.Parameter.FieldNames.In;
        public const string Style = OpenApi31.Parameter.FieldNames.Style;
        public const string Explode = OpenApi31.Parameter.FieldNames.Explode;
        public const string Schema = OpenApi31.Parameter.FieldNames.Schema;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    private Parameter(OpenApi31.Parameter inner)
    {
        Inner = inner;
    }

    internal OpenApi31.Parameter Inner { get; }

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
    public static Parameter Parse(string name, string style, string location, bool explode, IJsonSchema jsonSchema) =>
        new(OpenApi31.Parameter.Parse(name, style, location, explode, jsonSchema));

    /// <summary>
    /// The name of the parameter
    /// </summary>
    public string Name => Inner.Name;
    /// <summary>
    /// The style of the parameter
    /// </summary>
    public string Style => Inner.Style;
    /// <summary>
    /// If the parameter apply explosion of the serialized format
    /// </summary>
    public bool Explode => Inner.Explode;
    /// <summary>
    /// The parameter's json schema
    /// </summary>
    public IJsonSchema JsonSchema => Inner.JsonSchema;
}
