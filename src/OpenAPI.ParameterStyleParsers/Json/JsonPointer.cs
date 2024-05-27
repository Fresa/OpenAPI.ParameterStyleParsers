using System.Web;

namespace OpenAPI.ParameterStyleParsers.Json;

internal sealed class JsonPointer
{
    public IEnumerable<string> Segments { get; }

    private JsonPointer(IEnumerable<string> segments)
    {
        Segments = segments;
    }

    internal static JsonPointer Parse(string jsonPointer)
    {
        if (!jsonPointer.StartsWith('#'))
        {
            throw new NotSupportedException(
                "Only local JSON pointers are supported");
        }

        var segments = jsonPointer.Split('/');
        if (segments.First() != "#")
        {
            throw new InvalidOperationException(
                $"Invalid json pointer at position 1, expected '/': {jsonPointer}");
        }

        var jsonSegments =
            segments
                .Skip(1)
                .Select(segment =>
                    HttpUtility.UrlDecode(segment
                        .Replace("~1", "/")
                        .Replace("~0", "~")));

        return new JsonPointer(jsonSegments);
    }
}