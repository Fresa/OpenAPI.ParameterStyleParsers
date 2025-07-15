using System.Diagnostics.CodeAnalysis;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

namespace OpenAPI.ParameterStyleParsers.OpenApi20;

/// <summary>
/// An OpenAPI parameter specification
/// </summary>
public record Parameter
{
    /// <summary>
    /// Supported OpenAPI parameter collection formats
    /// </summary>
    public static class CollectionFormats
    {
#pragma warning disable CS1591
        public const string Csv = "csv";
        public const string Ssv = "ssv";
        public const string Tsv = "tsv";
        public const string Pipes = "pipes";
        public const string Multi = "multi";
        public static readonly string[] All = [Csv, Ssv, Tsv, Pipes, Multi];
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
        public const string FormData = "formData";
        public const string Body = "body";
        public static readonly string[] All = [Path, Header, Query, FormData, Body];
#pragma warning restore CS1591
    }

    /// <summary>
    /// A map between parameter locations and supported styles
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global Part of public contract
    public static readonly Dictionary<string, string[]> LocationToCollectionFormatMap = new()
    {
        [Locations.Path] = [CollectionFormats.Csv, CollectionFormats.Ssv, CollectionFormats.Tsv, CollectionFormats.Pipes],
        [Locations.Header] = [CollectionFormats.Csv, CollectionFormats.Ssv, CollectionFormats.Tsv, CollectionFormats.Pipes],
        [Locations.Query] = [CollectionFormats.Csv, CollectionFormats.Ssv, CollectionFormats.Tsv, CollectionFormats.Pipes, CollectionFormats.Multi],
        [Locations.FormData] = [CollectionFormats.Multi],
        [Locations.Body] = [],
    };

    /// <summary>
    /// Supported OpenAPI parameter types
    /// </summary>
    public static class Types
    {
#pragma warning disable CS1591
        public const string String = "string";
        public const string Number = "number";
        public const string Integer = "integer";
        public const string Boolean = "boolean";
        public const string Array = "array";
        public const string File = "file";
        public static readonly string[] All = [String, Number, Integer, Boolean, Array, File];
        public static readonly string[] Primitives = [String, Number, Integer, Boolean];
#pragma warning restore CS1591
    }
    
    /// <summary>
    /// Relevant field names for the parameter
    /// </summary>
    public static class FieldNames
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string Name = "name";
        public const string In = "in";
        public const string Type = "type";
        public const string CollectionFormat = "collectionFormat";
        public const string Items = "items";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    private Parameter(string name, string @in, string collectionFormat, string? type = null, ItemsObject? items = null)
    {
        Name = name;
        CollectionFormat = collectionFormat;
        IsBody = type == null;
        Type = type;
        Items = items;
        
        IsPath = @in == Locations.Path;
        IsFormData = @in == Locations.FormData;
        IsQuery = @in == Locations.Query;
        IsHeader = @in == Locations.Header;

        ValueIncludesKey = IsQuery || IsFormData;
    }


    /// <summary>
    /// Parses an OpenAPI parameter
    /// </summary>
    /// <param name="name">Parameter name</param>
    /// <param name="in">The location of the parameter</param>
    /// <param name="type">The type (null when parameter is body)</param>
    /// <param name="collectionFormat">Determines the format of the array if type array is used</param>
    /// <param name="items">The item directive is type is array</param>
    /// <returns>A parameter specification</returns>
    /// <exception cref="InvalidOperationException">Thrown if location and styles are incompatible</exception>
    public static Parameter Parse(string name, string @in, string? type, string collectionFormat = CollectionFormats.Csv, ItemsObject? items = null)
    {
        if (!LocationToCollectionFormatMap.TryGetValue(@in, out var collectionFormats))
        {
            throw new InvalidOperationException(
                $"Location '{@in}' is not a valid location. Valid locations are {string.Join(", ", Locations.All)}");
        }

        if (!collectionFormats.Contains(collectionFormat))
        {
            throw new InvalidOperationException(
                $"Location '{@in}' does not support collection format '{collectionFormat}'. Supported formats are {string.Join(", ", collectionFormats)}");
        }

        if (!Types.All.Contains(type))
        {
            throw new InvalidOperationException(
                $"Unknown type '{type}', expected one of {string.Join(", ", Types.All)}");
        }
        
        if (type == null && @in != Locations.Body)
        {
            throw new InvalidOperationException(
                $"Type cannot be null when location is '{@in}'. It must be any of {string.Join(", ", Types.All)}'");
        }

        if (type == Types.Array && items == null)
        {
            throw new InvalidOperationException(
                $"Items object cannot be null when type is '{type}'");
        }
        
        return new Parameter(name, collectionFormat, @in, type);
    }

    /// <summary>
    /// The name of the parameter
    /// </summary>
    public string Name { get; private init; }
    /// <summary>
    /// The style of the parameter
    /// </summary>
    public string CollectionFormat { get; private init; }
    /// <summary>
    /// Is the parameter the body directive?
    /// </summary>
    [MemberNotNullWhen(false, nameof(Type))] 
    public bool IsBody { get; private init; }
    /// <summary>
    /// The type of the parameter
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Is it an array type parameter?
    /// </summary>
    [MemberNotNullWhen(true, nameof(Items))]
    public bool IsArray => Type == Types.Array;
    
    /// <summary>
    /// Required if type is “array”. Describes the type of items in the array
    /// </summary>
    public ItemsObject? Items { get; private init; }
    
    /// <summary>
    /// Is the parameter located in the header?
    /// </summary>
    public bool IsHeader { get; }
    /// <summary>
    /// Is the parameter located in the path?
    /// </summary>
    public bool IsPath { get; }
    /// <summary>
    /// Is the parameter located in the query?
    /// </summary>
    public bool IsQuery { get; }
    /// <summary>
    /// Is the parameter located in the form data?
    /// </summary>
    public bool IsFormData { get; }
    
    /// <summary>
    /// Does the parameter value include keys, i.e. key=value
    /// </summary>
    public bool ValueIncludesKey { get; }
}
