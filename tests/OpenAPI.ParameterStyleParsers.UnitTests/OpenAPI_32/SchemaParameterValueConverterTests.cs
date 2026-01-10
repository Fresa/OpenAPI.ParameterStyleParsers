using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_32;

public class SchemaParameterValueConverterTests
{
    [Theory]
    [MemberData(nameof(CookiePrimitiveString))]
    [MemberData(nameof(CookiePrimitiveNumber))]
    [MemberData(nameof(CookiePrimitiveInteger))]
    [MemberData(nameof(CookiePrimitiveBoolean))]
    public void Given_a_cookie_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(CookiePrimitiveString))]
    [MemberData(nameof(CookiePrimitiveNumber))]
    [MemberData(nameof(CookiePrimitiveInteger))]
    [MemberData(nameof(CookiePrimitiveBoolean))]
    public void Given_a_cookie_primitive_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(CookieArrayExploded))]
    [MemberData(nameof(CookieArrayNonExploded))]
    public void Given_a_cookie_array_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(CookieArrayExploded))]
    [MemberData(nameof(CookieArrayNonExploded))]
    public void Given_a_cookie_array_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(CookieObjectExploded))]
    [MemberData(nameof(CookieObjectNonExploded))]
    public void Given_a_cookie_object_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(CookieObjectExploded))]
    [MemberData(nameof(CookieObjectNonExploded))]
    public void Given_a_cookie_object_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance);
    }

    private static void TestParsing(string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestParsing(parameterJson, values.First(), shouldMap, jsonInstance);
    }

    private static void TestParsing(string parameterJson,
        string? value,
        bool shouldMap,
        string? jsonInstance)
    {
        var parameter = OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        var parser = ParameterValueParser.Create(parameter);
        parser.TryParse(value, out var instance, out var mappingError).Should().Be(shouldMap, mappingError);
        if (!shouldMap)
        {
            mappingError.Should().NotBeNullOrEmpty();
            return;
        }

        if (instance is null)
        {
            jsonInstance.Should().BeNull();
        }
        else
        {
            jsonInstance.Should().NotBeNull();
            instance.ToJsonString().Should().BeEquivalentTo(jsonInstance);
        }
    }

    private static void TestSerializing(string parameterJson,
        string?[] expectedValues,
        bool shouldMap,
        string? jsonInstance)
    {
        if (!shouldMap)
            return;

        var parser = CreateParameterValueParser(parameterJson);
        var serialized = parser.Serialize(jsonInstance == null ? null : JsonNode.Parse(jsonInstance));

        expectedValues.Should().Contain(serialized);
    }

    private static ParameterValueParser CreateParameterValueParser(string parameterJson)
    {
        var parameter = OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        return ParameterValueParser.Create(parameter);
    }

    #region Cookie Primitives
    public static readonly TheoryData<string, string?[], bool, string?> CookiePrimitiveString = new()
    {
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
            ["session=abc123"],
            true,
            "\"abc123\""
        },
        {
            """
            {
                "name": "token",
                "in": "cookie",
                "schema": { "type": "string" },
                "style": "cookie",
                "explode": false
            }
            """,
            ["token=xyz"],
            true,
            "\"xyz\""
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> CookiePrimitiveNumber = new()
    {
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
            ["rate=3.14"],
            true,
            "3.14"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> CookiePrimitiveInteger = new()
    {
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
            ["count=42"],
            true,
            "42"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> CookiePrimitiveBoolean = new()
    {
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
            ["enabled=true"],
            true,
            "true"
        },
        {
            """
            {
                "name": "disabled",
                "in": "cookie",
                "schema": { "type": "boolean" },
                "style": "cookie",
                "explode": false
            }
            """,
            ["disabled=false"],
            true,
            "false"
        }
    };
    #endregion

    #region Cookie Arrays
    public static readonly TheoryData<string, string?[], bool, string?> CookieArrayExploded = new()
    {
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
            ["ids=a; ids=b; ids=c"],
            true,
            """["a","b","c"]"""
        },
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
            ["nums=1; nums=2; nums=3"],
            true,
            "[1,2,3]"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> CookieArrayNonExploded = new()
    {
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
            ["ids=a,b,c"],
            true,
            """["a","b","c"]"""
        },
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
            ["nums=1,2,3"],
            true,
            "[1,2,3]"
        }
    };
    #endregion

    #region Cookie Objects
    public static readonly TheoryData<string, string?[], bool, string?> CookieObjectExploded = new()
    {
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
            ["name=John; age=30"],
            true,
            """{"name":"John","age":30}"""
        },
        {
            """
            {
                "name": "point",
                "in": "cookie",
                "schema": {
                    "type": "object",
                    "properties": {
                        "x": { "type": "number" },
                        "y": { "type": "number" }
                    }
                },
                "style": "cookie",
                "explode": true
            }
            """,
            ["x=1.5; y=2.5"],
            true,
            """{"x":1.5,"y":2.5}"""
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> CookieObjectNonExploded = new()
    {
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
            ["user=name,John,age,30"],
            true,
            """{"name":"John","age":30}"""
        },
        {
            """
            {
                "name": "point",
                "in": "cookie",
                "schema": {
                    "type": "object",
                    "properties": {
                        "x": { "type": "number" },
                        "y": { "type": "number" }
                    }
                },
                "style": "cookie",
                "explode": false
            }
            """,
            ["point=x,1.5,y,2.5"],
            true,
            """{"x":1.5,"y":2.5}"""
        }
    };
    #endregion
}