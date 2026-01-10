using System.Text.Json.Nodes;
using AwesomeAssertions;
using OpenAPI.ParameterStyleParsers.OpenApi32;
using OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_32;

public class CookieStyleTests
{
    [Theory]
    [MemberData(nameof(PrimitiveData))]
    public void Given_a_cookie_primitive_parameter_When_parsing_It_should_parse_correctly(
        string parameterJson,
        string? value,
        bool shouldParse,
        string? expectedJson)
    {
        var parser = ParameterValueParserFactory.OpenApi32(parameterJson);
        var result = parser.TryParse(value, out var instance, out var error);

        result.Should().Be(shouldParse, error);
        if (shouldParse)
        {
            if (expectedJson == null)
                instance.Should().BeNull();
            else
                instance!.ToJsonString().Should().Be(expectedJson);
        }
    }

    [Theory]
    [MemberData(nameof(PrimitiveData))]
    public void Given_a_cookie_primitive_parameter_When_serializing_It_should_serialize_correctly(
        string parameterJson,
        string? expectedValue,
        bool shouldParse,
        string? jsonInstance)
    {
        if (!shouldParse) return;

        var parser = ParameterValueParserFactory.OpenApi32(parameterJson);
        var serialized = parser.Serialize(jsonInstance == null ? null : JsonNode.Parse(jsonInstance));

        serialized.Should().Be(expectedValue);
    }

    [Theory]
    [MemberData(nameof(ArrayData))]
    public void Given_a_cookie_array_parameter_When_parsing_It_should_parse_correctly(
        string parameterJson,
        string? value,
        bool shouldParse,
        string? expectedJson)
    {
        var parser = ParameterValueParserFactory.OpenApi32(parameterJson);
        var result = parser.TryParse(value, out var instance, out var error);

        result.Should().Be(shouldParse, error);
        if (shouldParse)
        {
            if (expectedJson == null)
                instance.Should().BeNull();
            else
                instance!.ToJsonString().Should().Be(expectedJson);
        }
    }

    [Theory]
    [MemberData(nameof(ArrayData))]
    public void Given_a_cookie_array_parameter_When_serializing_It_should_serialize_correctly(
        string parameterJson,
        string? expectedValue,
        bool shouldParse,
        string? jsonInstance)
    {
        if (!shouldParse) return;

        var parser = ParameterValueParserFactory.OpenApi32(parameterJson);
        var serialized = parser.Serialize(jsonInstance == null ? null : JsonNode.Parse(jsonInstance));

        serialized.Should().Be(expectedValue);
    }

    [Theory]
    [MemberData(nameof(ObjectData))]
    public void Given_a_cookie_object_parameter_When_parsing_It_should_parse_correctly(
        string parameterJson,
        string? value,
        bool shouldParse,
        string? expectedJson)
    {
        var parser = ParameterValueParserFactory.OpenApi32(parameterJson);
        var result = parser.TryParse(value, out var instance, out var error);

        result.Should().Be(shouldParse, error);
        if (shouldParse)
        {
            if (expectedJson == null)
                instance.Should().BeNull();
            else
                instance!.ToJsonString().Should().Be(expectedJson);
        }
    }

    [Theory]
    [MemberData(nameof(ObjectDataForSerialization))]
    public void Given_a_cookie_object_parameter_When_serializing_It_should_serialize_correctly(
        string parameterJson,
        string? expectedValue,
        bool shouldParse,
        string? jsonInstance)
    {
        if (!shouldParse) return;

        var parser = ParameterValueParserFactory.OpenApi32(parameterJson);
        var serialized = parser.Serialize(jsonInstance == null ? null : JsonNode.Parse(jsonInstance));

        serialized.Should().Be(expectedValue);
    }

    public static TheoryData<string, string?, bool, string?> PrimitiveData => new()
    {
        // String
        {
            """
            {
                "name": "session",
                "in": "cookie",
                "schema": { "type": "string" },
                "style": "cookie",
                "explode": true
            }
            """,
            "session=abc123",
            true,
            "\"abc123\""
        },
        // Integer
        {
            """
            {
                "name": "count",
                "in": "cookie",
                "schema": { "type": "integer" },
                "style": "cookie",
                "explode": true
            }
            """,
            "count=42",
            true,
            "42"
        },
        // Number
        {
            """
            {
                "name": "rate",
                "in": "cookie",
                "schema": { "type": "number" },
                "style": "cookie",
                "explode": true
            }
            """,
            "rate=3.14",
            true,
            "3.14"
        },
        // Boolean
        {
            """
            {
                "name": "enabled",
                "in": "cookie",
                "schema": { "type": "boolean" },
                "style": "cookie",
                "explode": true
            }
            """,
            "enabled=true",
            true,
            "true"
        },
        // Null
        {
            """
            {
                "name": "empty",
                "in": "cookie",
                "schema": { "type": "null" },
                "style": "cookie",
                "explode": true
            }
            """,
            null,
            true,
            null
        }
    };

    public static TheoryData<string, string?, bool, string?> ArrayData => new()
    {
        // Array exploded: "name=val1; name=val2"
        {
            """
            {
                "name": "ids",
                "in": "cookie",
                "schema": { "type": "array", "items": { "type": "string" } },
                "style": "cookie",
                "explode": true
            }
            """,
            "ids=a; ids=b; ids=c",
            true,
            """["a","b","c"]"""
        },
        // Array non-exploded: "name=val1,val2"
        {
            """
            {
                "name": "ids",
                "in": "cookie",
                "schema": { "type": "array", "items": { "type": "string" } },
                "style": "cookie",
                "explode": false
            }
            """,
            "ids=a,b,c",
            true,
            """["a","b","c"]"""
        },
        // Array with integers
        {
            """
            {
                "name": "nums",
                "in": "cookie",
                "schema": { "type": "array", "items": { "type": "integer" } },
                "style": "cookie",
                "explode": false
            }
            """,
            "nums=1,2,3",
            true,
            "[1,2,3]"
        },
        // Array exploded with integers
        {
            """
            {
                "name": "nums",
                "in": "cookie",
                "schema": { "type": "array", "items": { "type": "integer" } },
                "style": "cookie",
                "explode": true
            }
            """,
            "nums=1; nums=2; nums=3",
            true,
            "[1,2,3]"
        }
    };

    public static TheoryData<string, string?, bool, string?> ObjectData => new()
    {
        // Object exploded: "key1=val1; key2=val2"
        {
            """
            {
                "name": "user",
                "in": "cookie",
                "schema": {
                    "type": "object",
                    "properties": {
                        "name": { "type": "string" },
                        "age": { "type": "integer" }
                    }
                },
                "style": "cookie",
                "explode": true
            }
            """,
            "name=John; age=30",
            true,
            """{"name":"John","age":30}"""
        },
        // Object non-exploded: "param=key1,val1,key2,val2"
        {
            """
            {
                "name": "user",
                "in": "cookie",
                "schema": {
                    "type": "object",
                    "properties": {
                        "name": { "type": "string" },
                        "age": { "type": "integer" }
                    }
                },
                "style": "cookie",
                "explode": false
            }
            """,
            "user=name,John,age,30",
            true,
            """{"name":"John","age":30}"""
        }
    };

    // Separate data for serialization to control expected output order
    public static TheoryData<string, string?, bool, string?> ObjectDataForSerialization => new()
    {
        // Object exploded
        {
            """
            {
                "name": "user",
                "in": "cookie",
                "schema": {
                    "type": "object",
                    "properties": {
                        "name": { "type": "string" },
                        "age": { "type": "integer" }
                    }
                },
                "style": "cookie",
                "explode": true
            }
            """,
            "name=John; age=30",
            true,
            """{"name":"John","age":30}"""
        },
        // Object non-exploded
        {
            """
            {
                "name": "user",
                "in": "cookie",
                "schema": {
                    "type": "object",
                    "properties": {
                        "name": { "type": "string" },
                        "age": { "type": "integer" }
                    }
                },
                "style": "cookie",
                "explode": false
            }
            """,
            "user=name,John,age,30",
            true,
            """{"name":"John","age":30}"""
        }
    };
}