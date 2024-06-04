using System.Text.Json.Nodes;
using FluentAssertions;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.UnitTests;

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
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestSerializing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance);
    }


    [Theory]
    [MemberData(nameof(EmptySchema))]
    public void Given_a_parameter_with_empty_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(EmptySchema))]
    public void Given_a_primitive_parameter_with_empty_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] value,
        bool shouldMap,
        string? jsonInstance)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
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
        string? jsonInstance)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance);
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
        var parameterJsonNode = JsonNode.Parse(parameterJson)!;
        var reader = new JsonNodeReader(parameterJsonNode);
        var schema = new JsonSchema202012(parameterJsonNode["schema"]);
        var parameter =
            Parameter.Parse(
                reader.Read("name").GetValue<string>(),
                reader.Read("style").GetValue<string>(),
                reader.Read("in").GetValue<string>(),
                reader.Read("explode").GetValue<bool>(),
                schema);
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

    private static ParameterValueParser CreateParameterValueParser(
        string parameterJson)
    {
        var parameterJsonNode = JsonNode.Parse(parameterJson)!.AsObject();
        return ParameterValueParser.FromOpenApi31ParameterSpecification(parameterJsonNode);
    }

    #region Object
    public static readonly TheoryData<string, string?[], bool, string?> DeepObject = new()
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"R":"100","G":"200","B":""}"""
        }
    };
    public static readonly TheoryData<string, string?[], bool, string?> ObjectLabel = new()
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            "{}"
        }
    };
    public static readonly TheoryData<string, string?[], bool, string?> ObjectMatrix = new()
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"comma":",","dot":".","semi":";"}"""
        }
    };
    public static readonly TheoryData<string, string?[], bool, string?> ObjectForm = new()
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":100,"G":200,"B":""}"""
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ObjectSimple = new()
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":100,"G":200,"B":""}"""
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
            """{"R":100,"G":200,"B":150}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":"100","G":"200","B":""}"""
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
            """{"R":100,"G":200,"B":""}"""
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ObjectSpaceDelimited = new()
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
            new []{ "R%20100", "G%20200", "B%20150"}.GenerateAllPermutations("%20"),
            true,
            """{"R":100,"G":200,"B":150}"""
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
            new []{ "R%20100", "G%20200", "B%20"}.GenerateAllPermutations("%20"),
            true,
            """{"R":"100","G":"200","B":""}"""
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
            new []{ "R%20100", "G%20200", "B%20"}.GenerateAllPermutations("%20"),
            true,
            """{"R":"100","G":"200","B":""}"""
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
            new []{ "R%20100", "G%20200", "B%20"}.GenerateAllPermutations("%20"),
            true,
            """{"R":100,"G":200,"B":""}"""
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
            new []{ "R=100", "G=200", "B=150"}.GenerateAllPermutations("%20"),
            false,
            null
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ObjectPipeDelimited = new()
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
            new []{"R|100","G|200","B|150"}.GenerateAllPermutations('|'),
            true,
            """{"R":100,"G":200,"B":150}"""
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
            new []{"R|100","G|200","B|"}.GenerateAllPermutations('|'),
            true,
            """{"R":"100","G":"200","B":""}"""
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
            new []{"R|100","G|200","B|"}.GenerateAllPermutations('|'),
            true,
            """{"R":"100","G":"200","B":""}"""
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
            new []{"R|100","G|200","B|"}.GenerateAllPermutations('|'),
            true,
            """{"R":100,"G":200,"B":""}"""
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
            new []{"R|100","G|200","B|150"}.GenerateAllPermutations('|'),
            false,
            null
        }
    };
    #endregion

    #region Array
    public static readonly TheoryData<string, string?[], bool, string?> ArrayLabel = new()
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"\"]"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ArrayForm = new()
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"test2\"]"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ArrayMatrix = new()
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"\"]"
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
            "[\"test\",\"test2\"]"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ArraySimple = new()
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"\"]"
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
            "[\"test\",\"test2\"]"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ArraySpaceDelimited = new()
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"\"]"
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"\"]"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ArrayPipeDelimited = new()
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"\"]"
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
            "[\"test\",\"test2\"]"
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
            "[\"test\",\"\"]"
        }
    };
    #endregion

    public static readonly TheoryData<string, string?[], bool, string?> StringForm = new()
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
            "\"test\""
        }
    };

    public static TheoryData<string, string?[], bool, string?> NumberForm = new()
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
            "1.2"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> IntegerForm = new()
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
            "1"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> BooleanForm = new()
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
            "true"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> NullForm = new()
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
            null
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> StringLabel = new()
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
            "\"test\""
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> NumberLabel = new()
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
            "1.2"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> IntegerLabel = new()
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
            "1"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> BooleanLabel = new()
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
            "true"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> NullLabel = new()
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
            null
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
            null
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> StringMatrix = new()
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
            "\"test\""
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
            "\"test\""
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> NumberMatrix = new()
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
            "1.2"
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
            "1.2"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> IntegerMatrix = new()
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
            "1"
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
            "1"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> BooleanMatrix = new()
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
            "true"
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
            "true"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> NullMatrix = new()
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
            null
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
            null
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> StringSimple = new()
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
            "\"test\""
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
            "\"test\""
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> NumberSimple = new()
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
            "1.2"
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
            "1.2"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> IntegerSimple = new()
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
            "1"
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
            "1"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> BooleanSimple = new()
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
            "true"
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
            "true"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> NullSimple = new()
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
            null
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
            null
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> EmptySchema = new()
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
            """["test"]"""
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
            """["test","test2"]"""
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
            """[""]"""
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
            """[""]"""
        }
    };
}
