using System.Text.Json.Nodes;
using OpenAPI.ParameterStyleParsers.OpenApi32.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_32;

public class SchemaParameterValueConverterTests
{
    [Theory]
    [MemberData(nameof(HeaderPrimitiveString))]
    [MemberData(nameof(HeaderPrimitiveNumber))]
    [MemberData(nameof(HeaderPrimitiveInteger))]
    [MemberData(nameof(HeaderPrimitiveBoolean))]
    public void Given_a_header_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(HeaderPrimitiveString))]
    [MemberData(nameof(HeaderPrimitiveNumber))]
    [MemberData(nameof(HeaderPrimitiveInteger))]
    [MemberData(nameof(HeaderPrimitiveBoolean))]
    public void Given_a_header_primitive_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(HeaderArray))]
    public void Given_a_header_array_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(HeaderArray))]
    public void Given_a_header_array_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(HeaderObjectExploded))]
    [MemberData(nameof(HeaderObjectNonExploded))]
    public void Given_a_header_object_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(HeaderObjectExploded))]
    [MemberData(nameof(HeaderObjectNonExploded))]
    public void Given_a_header_object_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(CookiePrimitiveString))]
    [MemberData(nameof(CookiePrimitiveNumber))]
    [MemberData(nameof(CookiePrimitiveInteger))]
    [MemberData(nameof(CookiePrimitiveBoolean))]
    public void Given_a_cookie_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
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
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(CookieArrayExploded))]
    [MemberData(nameof(CookieArrayNonExploded))]
    public void Given_a_cookie_array_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(CookieArrayExploded))]
    [MemberData(nameof(CookieArrayNonExploded))]
    public void Given_a_cookie_array_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(CookieObjectExploded))]
    [MemberData(nameof(CookieObjectNonExploded))]
    public void Given_a_cookie_object_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(CookieObjectExploded))]
    [MemberData(nameof(CookieObjectNonExploded))]
    public void Given_a_cookie_object_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    private static void TestParsing(string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values.First(), shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    private static void TestParsing(string parameterJson,
        string? value,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        var parameter = OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        var parser = ParameterValueParser.Create(parameter);
        parser.ValueIncludesParameterName.Should().Be(expectedValueIncludesParameterName);
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
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        if (!shouldMap)
            return;

        var parser = CreateParameterValueParser(parameterJson);
        parser.ValueIncludesParameterName.Should().Be(expectedValueIncludesParameterName);
        var serialized = parser.Serialize(jsonInstance == null ? null : JsonNode.Parse(jsonInstance));

        expectedValues.Should().Contain(serialized);
    }

    private static ParameterValueParser CreateParameterValueParser(string parameterJson)
    {
        var parameter = OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        return ParameterValueParser.Create(parameter);
    }

    #region Header Primitives
    public static readonly TheoryData<string, string?[], bool, string?, bool> HeaderPrimitiveString = new()
    {
        {
            """
            {
                "name": "X-Token",
                "in": "header",
                "schema": { "type": "string" }
            }
            """,
            ["abc123"],
            true,
            "\"abc123\"",
            false
        },
        // Percent-encoding must NOT be decoded for headers in 3.2
        {
            """
            {
                "name": "X-Token",
                "in": "header",
                "schema": { "type": "string" }
            }
            """,
            ["hello%20world"],
            true,
            "\"hello%20world\"",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> HeaderPrimitiveNumber = new()
    {
        {
            """
            {
                "name": "X-Rate",
                "in": "header",
                "schema": { "type": "number" }
            }
            """,
            ["3.14"],
            true,
            "3.14",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> HeaderPrimitiveInteger = new()
    {
        {
            """
            {
                "name": "X-Count",
                "in": "header",
                "schema": { "type": "integer" }
            }
            """,
            ["42"],
            true,
            "42",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> HeaderPrimitiveBoolean = new()
    {
        {
            """
            {
                "name": "X-Enabled",
                "in": "header",
                "schema": { "type": "boolean" }
            }
            """,
            ["true"],
            true,
            "true",
            false
        },
        {
            """
            {
                "name": "X-Disabled",
                "in": "header",
                "schema": { "type": "boolean" }
            }
            """,
            ["false"],
            true,
            "false",
            false
        }
    };
    #endregion

    #region Header Arrays
    public static readonly TheoryData<string, string?[], bool, string?, bool> HeaderArray = new()
    {
        {
            """
            {
                "name": "X-Ids",
                "in": "header",
                "schema": { "type": "array", "items": { "type": "string" } }
            }
            """,
            ["a,b,c"],
            true,
            """["a","b","c"]""",
            false
        },
        {
            """
            {
                "name": "X-Nums",
                "in": "header",
                "schema": { "type": "array", "items": { "type": "integer" } }
            }
            """,
            ["1,2,3"],
            true,
            "[1,2,3]",
            false
        },
        // Percent-encoding must NOT be decoded for headers in 3.2
        {
            """
            {
                "name": "X-Values",
                "in": "header",
                "schema": { "type": "array", "items": { "type": "string" } }
            }
            """,
            ["hello%20world,foo%2Cbar"],
            true,
            """["hello%20world","foo%2Cbar"]""",
            false
        }
    };
    #endregion

    #region Header Objects
    public static readonly TheoryData<string, string?[], bool, string?, bool> HeaderObjectExploded = new()
    {
        {
            """
            {
                "name": "X-User",
                "in": "header",
                "schema": {
                    "type": "object",
                    "properties": {
                        "name": { "type": "string" },
                        "age": { "type": "integer" }
                    }
                },
                "explode": true
            }
            """,
            ["name=John,age=30"],
            true,
            """{"name":"John","age":30}""",
            false
        },
        // Percent-encoding must NOT be decoded for headers in 3.2
        {
            """
            {
                "name": "X-Data",
                "in": "header",
                "schema": {
                    "type": "object",
                    "properties": {
                        "key": { "type": "string" }
                    }
                },
                "explode": true
            }
            """,
            ["key=hello%20world"],
            true,
            """{"key":"hello%20world"}""",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> HeaderObjectNonExploded = new()
    {
        {
            """
            {
                "name": "X-User",
                "in": "header",
                "schema": {
                    "type": "object",
                    "properties": {
                        "name": { "type": "string" },
                        "age": { "type": "integer" }
                    }
                },
                "explode": false
            }
            """,
            ["name,John,age,30"],
            true,
            """{"name":"John","age":30}""",
            false
        },
        // Percent-encoding must NOT be decoded for headers in 3.2
        {
            """
            {
                "name": "X-Data",
                "in": "header",
                "schema": {
                    "type": "object",
                    "properties": {
                        "key": { "type": "string" }
                    }
                },
                "explode": false
            }
            """,
            ["key,hello%20world"],
            true,
            """{"key":"hello%20world"}""",
            false
        }
    };
    #endregion

    #region Cookie Primitives
    public static readonly TheoryData<string, string?[], bool, string?, bool> CookiePrimitiveString = new()
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
            "\"abc123\"",
            true
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
            "\"xyz\"",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> CookiePrimitiveNumber = new()
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
            "3.14",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> CookiePrimitiveInteger = new()
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
            "42",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> CookiePrimitiveBoolean = new()
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
            "true",
            true
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
            "false",
            true
        }
    };
    #endregion

    #region Cookie Arrays
    public static readonly TheoryData<string, string?[], bool, string?, bool> CookieArrayExploded = new()
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
            """["a","b","c"]""",
            true
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
            "[1,2,3]",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> CookieArrayNonExploded = new()
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
            """["a","b","c"]""",
            true
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
            "[1,2,3]",
            true
        }
    };
    #endregion

    #region Cookie Objects
    public static readonly TheoryData<string, string?[], bool, string?, bool> CookieObjectExploded = new()
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
            """{"name":"John","age":30}""",
            false
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
            """{"x":1.5,"y":2.5}""",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> CookieObjectNonExploded = new()
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
            """{"name":"John","age":30}""",
            true
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
            """{"x":1.5,"y":2.5}""",
            true
        }
    };
    #endregion
}