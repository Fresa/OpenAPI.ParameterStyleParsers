using System.Text.Json.Nodes;
using OpenApi30 = OpenAPI.ParameterStyleParsers.OpenApi30;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_30;

public class SchemaParameterValueConverterTests
{
    [Theory]
    [MemberData(nameof(StringForm))]
    [MemberData(nameof(NumberForm))]
    [MemberData(nameof(IntegerForm))]
    [MemberData(nameof(BooleanForm))]
    [MemberData(nameof(NullForm))]
    public void Given_a_form_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(StringForm))]
    [MemberData(nameof(NumberForm))]
    [MemberData(nameof(IntegerForm))]
    [MemberData(nameof(BooleanForm))]
    [MemberData(nameof(NullForm))]
    public void Given_a_form_primitive_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(StringLabel))]
    [MemberData(nameof(NumberLabel))]
    [MemberData(nameof(IntegerLabel))]
    [MemberData(nameof(BooleanLabel))]
    [MemberData(nameof(NullLabel))]
    public void Given_a_label_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(StringLabel))]
    [MemberData(nameof(NumberLabel))]
    [MemberData(nameof(IntegerLabel))]
    [MemberData(nameof(BooleanLabel))]
    [MemberData(nameof(NullLabel))]
    public void Given_a_label_primitive_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(StringMatrix))]
    [MemberData(nameof(NumberMatrix))]
    [MemberData(nameof(IntegerMatrix))]
    [MemberData(nameof(BooleanMatrix))]
    [MemberData(nameof(NullMatrix))]
    public void Given_a_matrix_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(StringMatrix))]
    [MemberData(nameof(NumberMatrix))]
    [MemberData(nameof(IntegerMatrix))]
    [MemberData(nameof(BooleanMatrix))]
    [MemberData(nameof(NullMatrix))]
    public void Given_a_matrix_primitive_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(StringSimple))]
    [MemberData(nameof(NumberSimple))]
    [MemberData(nameof(IntegerSimple))]
    [MemberData(nameof(BooleanSimple))]
    [MemberData(nameof(NullSimple))]
    public void Given_a_simple_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(StringSimple))]
    [MemberData(nameof(NumberSimple))]
    [MemberData(nameof(IntegerSimple))]
    [MemberData(nameof(BooleanSimple))]
    [MemberData(nameof(NullSimple))]
    public void Given_a_simple_primitive_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] value,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }


    [Theory]
    [MemberData(nameof(EmptySchema))]
    public void Given_a_parameter_with_empty_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(EmptySchema))]
    public void Given_a_primitive_parameter_with_empty_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] value,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(ArrayLabel))]
    [MemberData(nameof(ArrayForm))]
    [MemberData(nameof(ArrayMatrix))]
    [MemberData(nameof(ArraySimple))]
    [MemberData(nameof(ArraySpaceDelimited))]
    [MemberData(nameof(ArrayPipeDelimited))]
    public void Given_an_array_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(ArrayLabel))]
    [MemberData(nameof(ArrayForm))]
    [MemberData(nameof(ArrayMatrix))]
    [MemberData(nameof(ArraySimple))]
    [MemberData(nameof(ArraySpaceDelimited))]
    [MemberData(nameof(ArrayPipeDelimited))]
    public void Given_an_array_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] value,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(ObjectForm))]
    [MemberData(nameof(ObjectSimple))]
    [MemberData(nameof(ObjectMatrix))]
    [MemberData(nameof(ObjectLabel))]
    [MemberData(nameof(ObjectSpaceDelimited))]
    [MemberData(nameof(ObjectPipeDelimited))]
    [MemberData(nameof(DeepObject))]
    public void Given_a_object_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
    }

    [Theory]
    [MemberData(nameof(ObjectForm))]
    [MemberData(nameof(ObjectSimple))]
    [MemberData(nameof(ObjectMatrix))]
    [MemberData(nameof(ObjectLabel))]
    [MemberData(nameof(ObjectSpaceDelimited))]
    [MemberData(nameof(ObjectPipeDelimited))]
    [MemberData(nameof(DeepObject))]
    public void Given_an_object_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] value,
        bool shouldMap,
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance, expectedValueIncludesParameterName);
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
        var parameter = OpenApi30.Parameter.FromOpenApi30ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        var parser = OpenApi30.ParameterValueParser.Create(parameter);
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

        parser.ValueIncludesParameterName.Should().Be(expectedValueIncludesParameterName);
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
        var serialized = parser.Serialize(jsonInstance == null ? null : JsonNode.Parse(jsonInstance));

        expectedValues.Should().Contain(serialized);
        parser.ValueIncludesParameterName.Should().Be(expectedValueIncludesParameterName);
    }

    private static OpenApi30.ParameterValueParser CreateParameterValueParser(
        string parameterJson)
    {
        var parameter = OpenApi30.Parameter.FromOpenApi30ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        return OpenApi30.ParameterValueParser.Create(parameter);
    }

    #region Object
    public static readonly TheoryData<string, string?[], bool, string?, bool> DeepObject = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "deepObject",
                "explode": true
            }
            """,
            new []{ "color[R]=100", "color[G]=200", "color[B]=150"}.GenerateAllPermutations('&'),
            true,
            """{"R":100,"G":200,"B":150}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "deepObject",
                "explode": true
            }
            """,
            new []{ "color[R]=100", "color[G]=200", "color[B]="}.GenerateAllPermutations('&'),
            true,
            """{"R":"100","G":"200","B":""}""",
            true
        },
        // nullable: true allows null on object types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "nullable": true,
                    "properties": {
                        "R": {
                            "type": "number"
                        }
                    }
                },
                "style": "deepObject",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        }
    };
    public static readonly TheoryData<string, string?[], bool, string?, bool> ObjectLabel = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "label",
                "explode": true
            }
            """,
            new []{ "R=100", "G=200", "B=150"}.GenerateAllPermutations('.')
                .Select(str => $".{str}")
                .ToArray(),
            true,
            """{"R":100,"G":200,"B":150}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "label",
                "explode": true
            }
            """,
            new []{ "R=100", "G=200", "B="}.GenerateAllPermutations('.')
                .Select(str => $".{str}")
                .ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "label",
                "explode": false
            }
            """,
            new []{ "R.100", "G.200", "B.150"}.GenerateAllPermutations('.')
                .Select(str => $".{str}")
                .ToArray(),
            true,
            """{"R":100,"G":200,"B":150}""",
            false
        },
        {
        """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "label",
                "explode": false
            }
            """,
            new []{ "R.100", "G.200", "B."}.GenerateAllPermutations('.')
                .Select(str => $".{str}")
                .ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object"
                },
                "style": "label",
                "explode": false
            }
            """,
            [""],
            true,
            "{}",
            false
        }
    };
    public static readonly TheoryData<string, string?[], bool, string?, bool> ObjectMatrix = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "matrix",
                "explode": true
            }
            """,
            new []{ "R=100", "G=200", "B=150"}.GenerateAllPermutations(';')
                .Select(str => $";{str}")
                .ToArray(),
            true,
            """{"R":100,"G":200,"B":150}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "matrix",
                "explode": true
            }
            """,
            new []{ "R=100", "G=200", "B"}.GenerateAllPermutations(';')
                .Select(str => $";{str}")
                .ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "matrix",
                "explode": false
            }
            """,
            new []{ "R,100", "G,200", "B,150"}.GenerateAllPermutations(',')
                .Select(str => $";color={str}")
                .ToArray(),
            true,
            """{"R":100,"G":200,"B":150}""",
            true
        },
        {
            """
            {
                "name": "keys",
                "in": "path",
                "schema": {
                    "type": "object",
                    "properties": {
                        "semi": {
                            "type": "string"
                        },
                        "dot": {
                            "type": "string"
                        },
                        "comma": {
                            "type": "string"
                        }
                    }
                },
                "style": "matrix",
                "explode": false
            }
            """,
            new []{ "comma,%2C", "dot,.", "semi,%3B"}.GenerateAllPermutations(',')
                .Select(str => $";keys={str}")
                .ToArray(),
            true,
            """{"comma":",","dot":".","semi":";"}""",
            true
        }
    };
    public static readonly TheoryData<string, string?[], bool, string?, bool> ObjectForm = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "items": {
                        "type": "string"
                    },
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "form",
                "explode": false
            }
            """,
            new []{ "R,100", "G,200", "B,150"}.GenerateAllPermutations(',')
                .Select(str => $";color={str}")
                .ToArray(),
            true,
            """{"R":100,"G":200,"B":150}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "string"
                        },
                        "G": {
                            "type": "string"
                        },
                        "B": {
                            "type": "string"
                        }
                    }
                },
                "style": "form",
                "explode": false
            }
            """,
            new []{ "R,100", "G,200", "B,"}.GenerateAllPermutations(',')
                .Select(str => $";color={str}")
                .ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "form",
                "explode": false
            }
            """,
            new []{ "R,100", "G,200", "B,"}.GenerateAllPermutations(',')
                .Select(str => $";color={str}")
                .ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "patternProperties": {
                        "^R": {
                            "type": "number"
                        },
                        "^G": {
                            "type": "integer"
                        }
                    },
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "form",
                "explode": false
            }
            """,
            new []{ "R,100", "G,200", "B,"}.GenerateAllPermutations(',')
                .Select(str => $";color={str}")
                .ToArray(),
            true,
            """{"R":100,"G":200,"B":""}""",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ObjectSimple = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "items": {
                        "type": "string"
                    },
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "simple",
                "explode": false
            }
            """,
            new [] {"R,100","G,200","B,150"}.GenerateAllPermutations(','),
            true,
            """{"R":100,"G":200,"B":150}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "string"
                        },
                        "G": {
                            "type": "string"
                        },
                        "B": {
                            "type": "string"
                        }
                    }
                },
                "style": "simple",
                "explode": false
            }
            """,
            new []{"R,100","G,200","B,"}.GenerateAllPermutations(','),
            true,
            """{"R":"100","G":"200","B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": false
            }
            """,
            new []{"R,100","G,200","B,"}.GenerateAllPermutations(','),
            true,
            """{"R":"100","G":"200","B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "patternProperties": {
                        "^R": {
                            "type": "number"
                        },
                        "^G": {
                            "type": "integer"
                        }
                    },
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": false
            }
            """,
            new []{"R,100","G,200","B,"}.GenerateAllPermutations(','),
            true,
            """{"R":100,"G":200,"B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "items": {
                        "type": "string"
                    },
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            new []{"R=100","G=200","B=150"}.GenerateAllPermutations(','),
            true,
            """{"R":100,"G":200,"B":150}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "string"
                        },
                        "G": {
                            "type": "string"
                        },
                        "B": {
                            "type": "string"
                        }
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            new []{"R=100","G=200","B="}.GenerateAllPermutations(','),
            true,
            """{"R":"100","G":"200","B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            new []{"R=100","G=200","B="}.GenerateAllPermutations(','),
            true,
            """{"R":"100","G":"200","B":""}""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "patternProperties": {
                        "^R": {
                            "type": "number"
                        },
                        "^G": {
                            "type": "integer"
                        }
                    },
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            new []{"R=100","G=200","B="}.GenerateAllPermutations(','),
            true,
            """{"R":100,"G":200,"B":""}""",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ObjectSpaceDelimited = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "items": {
                        "type": "string"
                    },
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "spaceDelimited",
                "explode": false
            }
            """,
            new []{ "R%20100", "G%20200", "B%20150"}.GenerateAllPermutations("%20").Select(p => $"color={p}").ToArray(),
            true,
            """{"R":100,"G":200,"B":150}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "string"
                        },
                        "G": {
                            "type": "string"
                        },
                        "B": {
                            "type": "string"
                        }
                    }
                },
                "style": "spaceDelimited",
                "explode": false
            }
            """,
            new []{ "R%20100", "G%20200", "B%20"}.GenerateAllPermutations("%20").Select(p => $"color={p}").ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "spaceDelimited",
                "explode": false
            }
            """,
            new []{ "R%20100", "G%20200", "B%20"}.GenerateAllPermutations("%20").Select(p => $"color={p}").ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "patternProperties": {
                        "^R": {
                            "type": "number"
                        },
                        "^G": {
                            "type": "integer"
                        }
                    },
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "spaceDelimited",
                "explode": false
            }
            """,
            new []{ "R%20100", "G%20200", "B%20"}.GenerateAllPermutations("%20").Select(p => $"color={p}").ToArray(),
            true,
            """{"R":100,"G":200,"B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "items": {
                        "type": "string"
                    },
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "spaceDelimited",
                "explode": true
            }
            """,
            new []{ "R=100", "G=200", "B=150"}.GenerateAllPermutations("%20").Select(p => $"color={p}").ToArray(),
            false,
            null,
            true
        },
        // nullable: true allows null on object types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "nullable": true,
                    "properties": {
                        "R": {
                            "type": "number"
                        }
                    }
                },
                "style": "spaceDelimited",
                "explode": false
            }
            """,
            [null],
            true,
            null,
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ObjectPipeDelimited = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "items": {
                        "type": "string"
                    },
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "pipeDelimited",
                "explode": false
            }
            """,
            new []{"R|100","G|200","B|150"}.GenerateAllPermutations('|').Select(p => $"color={p}").ToArray(),
            true,
            """{"R":100,"G":200,"B":150}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "properties": {
                        "R": {
                            "type": "string"
                        },
                        "G": {
                            "type": "string"
                        },
                        "B": {
                            "type": "string"
                        }
                    }
                },
                "style": "pipeDelimited",
                "explode": false
            }
            """,
            new []{"R|100","G|200","B|"}.GenerateAllPermutations('|').Select(p => $"color={p}").ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "pipeDelimited",
                "explode": false
            }
            """,
            new []{"R|100","G|200","B|"}.GenerateAllPermutations('|').Select(p => $"color={p}").ToArray(),
            true,
            """{"R":"100","G":"200","B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "patternProperties": {
                        "^R": {
                            "type": "number"
                        },
                        "^G": {
                            "type": "integer"
                        }
                    },
                    "additionalProperties": {
                        "type": "string"
                    }
                },
                "style": "pipeDelimited",
                "explode": false
            }
            """,
            new []{"R|100","G|200","B|"}.GenerateAllPermutations('|').Select(p => $"color={p}").ToArray(),
            true,
            """{"R":100,"G":200,"B":""}""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "items": {
                        "type": "string"
                    },
                    "properties": {
                        "R": {
                            "type": "number"
                        },
                        "G": {
                            "type": "number"
                        },
                        "B": {
                            "type": "number"
                        }
                    }
                },
                "style": "pipeDelimited",
                "explode": true
            }
            """,
            new []{"R|100","G|200","B|150"}.GenerateAllPermutations('|').Select(p => $"color={p}").ToArray(),
            false,
            null,
            true
        },
        // nullable: true allows null on object types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "nullable": true,
                    "properties": {
                        "R": {
                            "type": "number"
                        }
                    }
                },
                "style": "pipeDelimited",
                "explode": false
            }
            """,
            [null],
            true,
            null,
            true
        }
    };
    #endregion

    #region Array
    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayLabel = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "label",
                "explode": true
            }
            """,
            [".test.test2"],
            true,
            "[\"test\",\"test2\"]",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "label",
                "explode": false
            }
            """,
            [".test.test2"],
            true,
            "[\"test\",\"test2\"]",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "label",
                "explode": false
            }
            """,
            [".test."],
            true,
            "[\"test\",\"\"]",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayForm = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "form",
                "explode": true
            }
            """,
            ["color=test&color=test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "form",
                "explode": false
            }
            """,
            ["color=test,test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        },
        // Percent-encoded values should be decoded
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "form",
                "explode": false
            }
            """,
            ["color=hello%20world,foo%2Cbar"],
            true,
            "[\"hello world\",\"foo,bar\"]",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayMatrix = new()
    {
        {
            """
            {
                "name": "test",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [";test=test;test=test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        },
        {
            """
            {
                "name": "test",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [";test=test;test"],
            true,
            "[\"test\",\"\"]",
            true
        },
        {
            """
            {
                "name": "test",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "matrix",
                "explode": false
            }
            """,
            [";test=test,test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ArraySimple = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            ["test,test2"],
            true,
            "[\"test\",\"test2\"]",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            ["test,"],
            true,
            "[\"test\",\"\"]",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": false
            }
            """,
            ["test,test2"],
            true,
            "[\"test\",\"test2\"]",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ArraySpaceDelimited = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "spaceDelimited",
                "explode": true
            }
            """,
            ["color=test&color=test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "spaceDelimited",
                "explode": true
            }
            """,
            ["color=test&color="],
            true,
            "[\"test\",\"\"]",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "spaceDelimited",
                "explode": false
            }
            """,
            ["color=test%20test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "spaceDelimited",
                "explode": false
            }
            """,
            ["color=test%20"],
            true,
            "[\"test\",\"\"]",
            true
        },
        // nullable: true allows null on array types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "nullable": true,
                    "items": {
                        "type": "string"
                    }
                },
                "style": "spaceDelimited",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayPipeDelimited = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "pipeDelimited",
                "explode": true
            }
            """,
            ["color=test&color=test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "pipeDelimited",
                "explode": true
            }
            """,
            ["color=test&color="],
            true,
            "[\"test\",\"\"]",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "pipeDelimited",
                "explode": false
            }
            """,
            ["color=test|test2"],
            true,
            "[\"test\",\"test2\"]",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "style": "pipeDelimited",
                "explode": false
            }
            """,
            ["color=test|"],
            true,
            "[\"test\",\"\"]",
            true
        },
        // nullable: true allows null on array types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "nullable": true,
                    "items": {
                        "type": "string"
                    }
                },
                "style": "pipeDelimited",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        }
    };
    #endregion

    public static readonly TheoryData<string, string?[], bool, string?, bool> StringForm = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "string"
                },
                "style": "form",
                "explode": true
            }
            """,
            ["color=test"],
            true,
            "\"test\"",
            true
        }
    };

    public static TheoryData<string, string?[], bool, string?, bool> NumberForm = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "number"
                },
                "style": "form",
                "explode": true
            }
            """,
            ["color=1.2"],
            true,
            "1.2",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> IntegerForm = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "integer"
                },
                "style": "form",
                "explode": true
            }
            """,
            ["color=1"],
            true,
            "1",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> BooleanForm = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "boolean"
                },
                "style": "form",
                "explode": true}
            """,
            ["color=true"],
            true,
            "true",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> NullForm = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "null"
                },
                "style": "form",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        // nullable: true allows null on other types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "string",
                    "nullable": true
                },
                "style": "form",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "number",
                    "nullable": true
                },
                "style": "form",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "integer",
                    "nullable": true
                },
                "style": "form",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "boolean",
                    "nullable": true
                },
                "style": "form",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "array",
                    "nullable": true,
                    "items": {
                        "type": "string"
                    }
                },
                "style": "form",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                    "type": "object",
                    "nullable": true,
                    "properties": {
                        "R": {
                            "type": "number"
                        }
                    }
                },
                "style": "form",
                "explode": false
            }
            """,
            [null],
            true,
            null,
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> StringLabel = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "string"
                },
                "style": "label",
                "explode": true
            }
            """,
            [".test"],
            true,
            "\"test\"",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> NumberLabel = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "number"
                },
                "style": "label",
                "explode": true
            }
            """,
            [".1.2"],
            true,
            "1.2",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> IntegerLabel = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "integer"
                },
                "style": "label",
                "explode": true
            }
            """,
            [".1"],
            true,
            "1",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> BooleanLabel = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "boolean"
                },
                "style": "label",
                "explode": true}
            """,
            [".true"],
            true,
            "true",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> NullLabel = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "label",
                "explode": false
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "label",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        // nullable: true allows null on other types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "string",
                    "nullable": true
                },
                "style": "label",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "number",
                    "nullable": true
                },
                "style": "label",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "integer",
                    "nullable": true
                },
                "style": "label",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "boolean",
                    "nullable": true
                },
                "style": "label",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "nullable": true,
                    "items": {
                        "type": "string"
                    }
                },
                "style": "label",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "nullable": true,
                    "properties": {
                        "R": {
                            "type": "number"
                        }
                    }
                },
                "style": "label",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> StringMatrix = new()
    {
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "string"
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [";foo=test"],
            true,
            "\"test\"",
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "string"
                },
                "style": "matrix",
                "explode": false
            }
            """,
            [";foo=test"],
            true,
            "\"test\"",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> NumberMatrix = new()
    {
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "number"
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [";foo=1.2"],
            true,
            "1.2",
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "number"
                },
                "style": "matrix",
                "explode": false
            }
            """,
            [";foo=1.2"],
            true,
            "1.2",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> IntegerMatrix = new()
    {
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "integer"
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [";foo=1"],
            true,
            "1",
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "integer"
                },
                "style": "matrix",
                "explode": false
            }
            """,
            [";foo=1"],
            true,
            "1",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> BooleanMatrix = new()
    {
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "boolean"
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [";foo=true"],
            true,
            "true",
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "boolean"
                },
                "style": "matrix",
                "explode": false
            }
            """,
            [";foo=true"],
            true,
            "true",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> NullMatrix = new()
    {
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "matrix",
                "explode": false
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        // nullable: true allows null on other types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "string",
                    "nullable": true
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "number",
                    "nullable": true
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "integer",
                    "nullable": true
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "boolean",
                    "nullable": true
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "array",
                    "nullable": true,
                    "items": {
                        "type": "string"
                    }
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            true
        },
        {
            """
            {
                "name": "foo",
                "in": "path",
                "schema": {
                    "type": "object",
                    "nullable": true,
                    "properties": {
                        "R": {
                            "type": "number"
                        }
                    }
                },
                "style": "matrix",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> StringSimple = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "string"
                },
                "style": "simple",
                "explode": true
            }
            """,
            ["test"],
            true,
            "\"test\"",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "string"
                },
                "style": "simple",
                "explode": false
            }
            """,
            ["test"],
            true,
            "\"test\"",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> NumberSimple = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "number"
                },
                "style": "simple",
                "explode": true
            }
            """,
            ["1.2"],
            true,
            "1.2",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "number"
                },
                "style": "simple",
                "explode": false
            }
            """,
            ["1.2"],
            true,
            "1.2",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> IntegerSimple = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "integer"
                },
                "style": "simple",
                "explode": true
            }
            """,
            ["1"],
            true,
            "1",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "integer"
                },
                "style": "simple",
                "explode": false
            }
            """,
            ["1"],
            true,
            "1",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> BooleanSimple = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "boolean"
                },
                "style": "simple",
                "explode": true
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
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "boolean"
                },
                "style": "simple",
                "explode": false
            }
            """,
            ["true"],
            true,
            "true",
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> NullSimple = new()
    {
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "simple",
                "explode": false
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "simple",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        // nullable: true allows null on other types (OpenAPI 3.0 extension)
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "string",
                    "nullable": true
                },
                "style": "simple",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "number",
                    "nullable": true
                },
                "style": "simple",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "integer",
                    "nullable": true
                },
                "style": "simple",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "boolean",
                    "nullable": true
                },
                "style": "simple",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "array",
                    "nullable": true,
                    "items": {
                        "type": "string"
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                    "type": "object",
                    "nullable": true,
                    "properties": {
                        "R": {
                            "type": "number"
                        }
                    }
                },
                "style": "simple",
                "explode": true
            }
            """,
            [null],
            true,
            null,
            false
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> EmptySchema = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                },
                "style": "form",
                "explode": true
            }
            """,
            ["color=test"],
            true,
            """["test"]""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "query",
                "schema": {
                },
                "style": "form",
                "explode": true
            }
            """,
            ["color=test&color=test2"],
            true,
            """["test","test2"]""",
            true
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                },
                "style": "simple",
                "explode": false
            }
            """,
            [""],
            true,
            """[""]""",
            false
        },
        {
            """
            {
                "name": "color",
                "in": "path",
                "schema": {
                },
                "style": "label",
                "explode": false
            }
            """,
            ["."],
            true,
            """[""]""",
            false
        }
    };
}