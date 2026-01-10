using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Json.More;
using Json.Pointer;

namespace OpenAPI.ParameterStyleParsers.UnitTests;

internal sealed class JsonNodeReader
{
    private readonly JsonNode _root;
    private readonly ConcurrentDictionary<JsonPointer, JsonNodeReader?> _nodeCache = new();
    private static readonly JsonPointer RefPointer = JsonPointer.Create("$ref");

    internal JsonNodeReader(JsonNode root)
    {
        _root = root;
        RootPath = JsonPointer.Parse(root.GetPointerFromRoot());
    }

    /// <summary>
    /// The absolute path to this node from root
    /// </summary>
    internal JsonPointer RootPath { get; }

    internal JsonNodeReader Read(params string[] pointerSegments) =>
        Read(JsonPointer.Parse("/" + string.Join('/', pointerSegments)));

    internal JsonNodeReader Read(JsonPointer pointer)
    {
        if (!TryRead(pointer, out var reader))
        {
            throw new InvalidOperationException(
                $"{pointer} does not exist in json {_root}");
        }
        return reader;
    }

    internal bool TryRead(string segment, [NotNullWhen(true)] out JsonNodeReader? reader)
        => TryRead(JsonPointer.Parse("/" + segment), out reader);
    internal bool TryRead(JsonPointer pointer, [NotNullWhen(true)] out JsonNodeReader? reader)
    {
        reader = _nodeCache.GetOrAdd(pointer, TryRead(pointer));
        return reader != null;
    }

    private JsonNodeReader? TryRead(JsonPointer pointer)
    {
        var reader = ResolveReferences();

        if (!pointer.TryEvaluate(reader._root, out var node) ||
            node == null)
        {
            return null;
        }

        return new JsonNodeReader(node);
    }

    private JsonNodeReader ResolveReferences()
    {
        if (!RefPointer.TryEvaluate(_root, out var referenceNode))
        {
            return this;
        }

        var referencePointerExpression = referenceNode!.GetValue<string>();
        if (!referencePointerExpression.StartsWith("#"))
            throw new InvalidOperationException("Only local (fragment) $ref pointers are supported");

        var referencePointer = JsonPointer.Parse(referencePointerExpression);
        var reader = new JsonNodeReader(_root.Root).Read(
            referencePointer);
        return reader.ResolveReferences();
    }

    internal T GetValue<T>() => _root.GetValue<T>();
}