using System.Text.RegularExpressions;

namespace OpenAPI.ParameterStyleParsers.JsonSchema;

/// <summary>
/// A json schema
/// </summary>
public interface IJsonSchema
{
    /// <summary>
    /// Get the instance type defined in this schema
    /// </summary>
    InstanceType? GetInstanceType();

    /// <summary>
    /// Get the schema for any items defined in this schema
    /// </summary>
    /// <returns></returns>
    IJsonSchema? GetItems();
    /// <summary>
    /// Get the schemas for any properties defined in this schema
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string, IJsonSchema>? GetProperties();
    /// <summary>
    /// Get any additional properties defined in this schema
    /// </summary>
    /// <returns></returns>
    IJsonSchema? GetAdditionalProperties();

    /// <summary>
    /// Get any pattern properties defined in this schema
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<Regex, IJsonSchema>? GetPatternProperties();
}