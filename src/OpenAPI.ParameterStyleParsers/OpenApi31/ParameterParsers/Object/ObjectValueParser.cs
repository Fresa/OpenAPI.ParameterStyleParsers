using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Object;

internal abstract class ObjectValueParser(Parameter parameter) : IValueParser
{
    private readonly PropertySchemaResolver _propertySchemaResolver = new(parameter.JsonSchema);

    protected bool Explode { get; } = parameter.Explode;
    internal abstract bool ValueIncludesParameterName { get; }
    protected string ParameterName { get; } = parameter.Name;

    internal static ObjectValueParser Create(Parameter parameter) =>
        parameter.Style switch
        {
            Parameter.Styles.Matrix => new MatrixObjectValueParser(parameter),
            Parameter.Styles.Label => new LabelObjectValueParser(parameter),
            Parameter.Styles.Form => new FormObjectValueParser(parameter),
            Parameter.Styles.Simple => new SimpleObjectValueParser(parameter),
            Parameter.Styles.DeepObject => new DeepObjectValueParser(parameter),
            Parameter.Styles.PipeDelimited => new PipeDelimitedObjectValueParser(parameter),
            Parameter.Styles.SpaceDelimited => new SpaceDelimitedObjectValueParser(parameter),
            _ => throw new ArgumentException(nameof(parameter.Style),
                $"Style '{parameter.Style}' not supported for object")
        };

    public abstract bool TryParse(
        string? value,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error);

    public string? Serialize(JsonNode? instance)
    {
        if (instance == null)
        {
            return null;
        }

        var properties = instance
            .AsObject()
            .ToImmutableDictionary(
                property => property.Key,
                property =>
                    property.Value == null ? null : Uri.EscapeDataString(property.Value.ToString()));
        return Serialize(properties);
    }

    protected abstract string Serialize(IDictionary<string, string?> properties);

    protected bool TryGetObjectProperties(
        IReadOnlyList<string>? keyAndValues,
        out JsonNode? obj,
        [NotNullWhen(false)] out string? error)
    {
        if (keyAndValues == null)
        {
            obj = null;
            error = null;
            return true;
        }

        var jsonObject = new JsonObject();
        for (var i = 0; i < keyAndValues.Count; i += 2)
        {
            var propertyName = keyAndValues[i];
            var propertyValue = Uri.UnescapeDataString(keyAndValues.Count == i + 1 ? string.Empty : keyAndValues[i + 1]);
            _propertySchemaResolver.TryGetSchemaForProperty(propertyName, out var propertySchema);
            // Undefined type? Use string as default as any value should be valid
            var jsonType = propertySchema?.GetInstanceType() ?? InstanceType.String;

            if (!PrimitiveJsonConverter.TryConvert(propertyValue, jsonType, out var value, out error))
            {
                obj = null;
                return false;
            }
            jsonObject[propertyName] = value;
        }

        obj = jsonObject;
        error = null;
        return true;
    }
}