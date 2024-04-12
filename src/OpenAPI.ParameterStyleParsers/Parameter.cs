using Json.Schema;

namespace OpenAPI.ParameterStyleParsers;

public record Parameter
{
    public static class Styles
    {
        public const string Matrix = "matrix";
        public const string Label = "label";
        public const string Simple = "simple";
        public const string Form = "form";
        public const string SpaceDelimited = "spaceDelimited";
        public const string PipeDelimited = "pipeDelimited";
        public const string DeepObject = "deepObject";
    }

    public static class Locations
    {
        public const string Path = "path";
        public const string Header = "header";
        public const string Query = "query";
        public const string Cookie = "cookie";
    }

    public static readonly Dictionary<string, string[]> LocationToStylesMap = new()
    {
        [Locations.Path] = [Styles.Matrix, Styles.Label, Styles.Simple],
        [Locations.Header] = [Styles.Simple],
        [Locations.Query] = [Styles.Form, Styles.SpaceDelimited, Styles.PipeDelimited, Styles.DeepObject],
        [Locations.Cookie] = [Styles.Form],
    };

    private Parameter(string name, string style, bool explode, JsonSchema jsonSchema)
    {
        Name = name;
        Style = style;
        Explode = explode;
        JsonSchema = jsonSchema;
    }

    public static Parameter Parse(string name, string style, string location, bool explode, JsonSchema jsonSchema)
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

    public string Name { get; private init; }
    public string Style { get; private init; }
    public bool Explode { get; private init; }
    public JsonSchema JsonSchema { get; private init; }
}