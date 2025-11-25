using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.Json;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

/// <summary>
/// An Items Objects specification
/// <see href="https://spec.openapis.org/oas/v2.0#items-object" />
/// </summary>
public record ItemsObject
{
    /// <summary>
    /// Supported OpenAPI items object types
    /// </summary>
    public static class Types
    {
#pragma warning disable CS1591
        public const string String = "string";
        public const string Number = "number";
        public const string Integer = "integer";
        public const string Boolean = "boolean";
        public static readonly string[] All = [String, Number, Integer, Boolean];
#pragma warning restore CS1591
    }
    
    /// <summary>
    /// Relevant field names for the items object
    /// </summary>
    public static class FieldNames
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string Type = "type";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    private ItemsObject(string type)
    {
        Type = type;
    }

    /// <summary>
    /// Parses an OpenAPI Items object
    /// </summary>
    /// <param name="type">The type of the item</param>
    /// <returns>An items object specification</returns>
    /// <exception cref="InvalidOperationException">Thrown if the parameters are incompatible</exception>
    public static ItemsObject Parse(string type)
    {
        if (!Types.All.Contains(type))
        {
            throw new InvalidOperationException(
                $"type '{type}' is not a valid type. Valid types are {string.Join(", ", Types.All)}");
        }

        return new ItemsObject(type);
    }

    /// <summary>
    /// Parses an items object from json
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static ItemsObject FromOpenApi20ItemsObjectSpecification(JsonObject items)
    {
        var type = items.GetRequiredPropertyValue<string>(FieldNames.Type);
        return Parse(type);
    }
    
    /// <summary>
    /// Required. The internal type of the array. The value MUST be one of "string", "number", "integer", "boolean". Files, models and arrays are not allowed.
    /// </summary>
    public string Type { get; }
}
