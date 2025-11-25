using System.Text.Json.Nodes;
using Json.More;
using OpenAPI.ParameterStyleParsers.Json;

namespace OpenAPI.ParameterStyleParsers.UnitTests;

public class JsonNodeRefResolveTests
{
    [Theory]
    [MemberData(nameof(ValidReferences))]
    public void Given_a_json_object_when_pointing_to_a_valid_reference_it_should_resolve_it(
        string pointer, 
        string expectedJson)
    {
        var jsonPointer = JsonPointer.Parse(pointer);
        var resolvedJsonNode = JsonExample.Resolve(jsonPointer);
        resolvedJsonNode.IsEquivalentTo(
                JsonNode.Parse(expectedJson))
            .Should().BeTrue();
    }

    private static readonly JsonNode JsonExample = JsonNode.Parse(
        """
        {
            "foo": ["bar", "baz"],
            "": 0,
            "a/b": 1,
            "c%d": 2,
            "e^f": 3,
            "g|h": 4,
            "i\\j": 5,
            "k\"l": 6,
            " ": 7,
            "m~n": 8
        }
        """)!;

    public static readonly TheoryData<string, string> ValidReferences = new()
    {
        { "#", """
               {
                   "foo": ["bar", "baz"],
                   "": 0,
                   "a/b": 1,
                   "c%d": 2,
                   "e^f": 3,
                   "g|h": 4,
                   "i\\j": 5,
                   "k\"l": 6,
                   " ": 7,
                   "m~n": 8
               }
               """ },
        { "#/foo", """["bar", "baz"]""" },
        { "#/foo/0", "\"bar\"" },
        { "#/", "0" },
        { "#/a~1b", "1" },
        { "#/c%25d", "2" },
        { "#/e%5Ef", "3" },
        { "#/g%7Ch", "4" },
        { "#/i%5Cj", "5" },
        { "#/k%22l", "6" },
        { "#/%20", "7" },
        { "#/m~0n", "8" }
    };
}