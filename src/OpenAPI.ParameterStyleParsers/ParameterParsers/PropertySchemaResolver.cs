using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using OpenAPI.ParameterStyleParsers.JsonSchema;

namespace OpenAPI.ParameterStyleParsers.ParameterParsers;

internal sealed class PropertySchemaResolver
{
    private readonly IReadOnlyDictionary<string, IJsonSchema>? _propertySchemas;
    private readonly IJsonSchema? _additionalPropertiesSchema;
    private readonly IReadOnlyDictionary<Regex, IJsonSchema>? _patternPropertySchemas;

    public PropertySchemaResolver(IJsonSchema schema)
    {
        _propertySchemas = schema.GetProperties();
        _additionalPropertiesSchema = schema.GetAdditionalProperties();
        _patternPropertySchemas = schema.GetPatternProperties();
    }

    public bool TryGetSchemaForProperty(string propertyName, [NotNullWhen(true)] out IJsonSchema? schema)
    {
        if (_propertySchemas?.TryGetValue(propertyName, out schema) ?? false)
            return true;

        if (_patternPropertySchemas != null)
        {
            foreach (var (pattern, patternSchema) in _patternPropertySchemas)
            {
                if (pattern.Match(propertyName).Success)
                {
                    schema = patternSchema;
                    return true;
                }
            }
        }

        if (_additionalPropertiesSchema != null)
        {
            schema = _additionalPropertiesSchema;
            return true;
        }

        schema = null;
        return false;
    }
}