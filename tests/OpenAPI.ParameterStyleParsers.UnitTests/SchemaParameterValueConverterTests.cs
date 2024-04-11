using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Json.Pointer;
using Json.Schema;
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
        string? value,
        bool shouldMap,
        string? jsonInstance)
    {
        Test(parameterJson, value, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(StringLabel))]
    [MemberData(nameof(NumberLabel))]
    [MemberData(nameof(IntegerLabel))]
    [MemberData(nameof(BooleanLabel))]
    [MemberData(nameof(NullLabel))]
    public void Given_a_label_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string? value,
        bool shouldMap,
        string? jsonInstance)
    {
        Test(parameterJson, value, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(EmptySchema))]
    public void Given_a_parameter_with_empty_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string? value,
        bool shouldMap,
        string? jsonInstance)
    {
        Test(parameterJson, value, shouldMap, jsonInstance);
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
        string? value,
        bool shouldMap,
        string? jsonInstance)
    {
        Test(parameterJson, value, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(ObjectForm))]
    [MemberData(nameof(ObjectSimple))]
    [MemberData(nameof(ObjectMatrix))]
    [MemberData(nameof(ObjectLabel))]
    [MemberData(nameof(DeepObject))]
    public void Given_a_object_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string? value,
        bool shouldMap,
        string? jsonInstance)
    {
        Test(parameterJson, value, shouldMap, jsonInstance);
    }

    private static void Test(string parameterJson,
        string? value,
        bool shouldMap,
        string? jsonInstance)
    {
        var parameterJsonNode = JsonNode.Parse(parameterJson)!;
        var reader = new JsonNodeReader(parameterJsonNode, JsonPointer.Empty);
        var schema = parameterJsonNode["schema"].Deserialize<JsonSchema>() ??
                     throw new InvalidOperationException("json schema is missing");
        var parameter =
            Parameter.Parse(
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

    #region Object
    public static TheoryData<string, string, bool, string?> DeepObject => new()
    {
        {
            """
            {
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
            "color[R]=100&color[G]=200&color[B]=150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            "color[R]=100&color[G]=200&color[B]=",
            true,
            """{"R":"100","G":"200","B":""}"""
        }
    };
    public static TheoryData<string, string, bool, string?> ObjectLabel => new()
    {
        {
            """
            {
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
            ".R=100.G=200.B=150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            ".R=100.G=200.B",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            ".R.100.G.200.B.150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
        """
            {
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
        ".R.100.G.200.B.",
        true,
        """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "object"
                },
                "style": "label",
                "explode": false
            }
            """,
            "",
            true,
            "{}"
        }
    };
    public static TheoryData<string, string, bool, string?> ObjectMatrix => new()
    {
        {
            """
            {
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
            ";R=100;G=200;B=150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            ";R=100;G=200;B",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            ";color=R,100,G,200,B,150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            ";keys=comma,%2C,dot,.,semi,%3B",
            true,
            """{"comma":",","dot":".","semi":";"}"""
        }
    };
    public static TheoryData<string, string, bool, string?> ObjectForm => new()
    {
        {
            """
            {
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
            "color=R,100,G,200,B,150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            "color=R,100,G,200,B,",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            "color=R,100,G,200,B,",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            "color=R,100,G,200,B,",
            true,
            """{"R":100,"G":200,"B":""}"""
        }
    };

    public static TheoryData<string, string, bool, string?> ObjectSimple => new()
    {
        {
            """
            {
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
            "R,100,G,200,B,150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            "R,100,G,200,B,",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            "R,100,G,200,B,",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            "R,100,G,200,B,",
            true,
            """{"R":100,"G":200,"B":""}"""
        },
        {
            """
            {
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
            "R=100,G=200,B=150",
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            "R=100,G=200,B=",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            "R=100,G=200,B=",
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            "R=100,G=200,B=",
            true,
            """{"R":100,"G":200,"B":""}"""
        }
    };
    #endregion

    #region Array
    public static TheoryData<string, string?, bool, string?> ArrayLabel => new()
    {
        {
            """
            {
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
            ".test.test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            ".test.test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            ".test.",
            true,
            "[\"test\",\"\"]"
        }
    };

    public static TheoryData<string, string?, bool, string?> ArrayForm => new()
    {
        {
            """
            {
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
            "color=test&color=test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            "color=test,test2",
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static TheoryData<string, string?, bool, string?> ArrayMatrix => new()
    {
        {
            """
            {
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
            ";test=test;test=test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            ";test=test;test",
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            ";test=test,test2",
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static TheoryData<string, string?, bool, string?> ArraySimple => new()
    {
        {
            """
            {
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
            "test,test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            "test,",
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            "test,test2",
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static TheoryData<string, string?, bool, string?> ArraySpaceDelimited => new()
    {
        {
            """
            {
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
            "color=test&color=test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            "color=test&color=",
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            "color=test%20test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            "color=test%20",
            true,
            "[\"test\",\"\"]"
        }
    };

    public static TheoryData<string, string?, bool, string?> ArrayPipeDelimited => new()
    {
        {
            """
            {
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
            "color=test&color=test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            "color=test&color=",
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            "color=test|test2",
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            "color=test|",
            true,
            "[\"test\",\"\"]"
        }
    };
    #endregion

    public static TheoryData<string, string, bool, string?> StringForm => new()
    {
        {
            """
            {
                "in": "query",
                "schema": {
                    "type": "string" 
                },
                "style": "form",
                "explode": true
            }
            """,
            "test",
            true,
            "\"test\""
        }
    };

    public static TheoryData<string, string, bool, string?> NumberForm => new()
    {
        {
            """
            {
                "in": "query",
                "schema": {
                    "type": "number" 
                },
                "style": "form",
                "explode": true
            }
            """,
            "1.2",
            true,
            "1.2"
        }
    };

    public static TheoryData<string, string, bool, string?> IntegerForm => new()
    {
        {
            """
            {
                "in": "query",
                "schema": {
                    "type": "integer" 
                },
                "style": "form",
                "explode": true
            }
            """,
            "1",
            true,
            "1"
        }
    };

    public static TheoryData<string, string, bool, string?> BooleanForm => new()
    {
        {
            """
            {
                "in": "query",
                "schema": {
                    "type": "boolean" 
                },
                "style": "form",
                "explode": true}
            """,
            "true",
            true,
            "true"
        }
    };

    public static TheoryData<string, string?, bool, string?> NullForm => new()
    {
        {
            """
            {
                "in": "query",
                "schema": {
                    "type": "null" 
                },
                "style": "form",
                "explode": true
            }
            """,
            "",
            true,
            null
        },
        {
            """
            {
                "in": "query",
                "schema": {
                    "type": "null"
                },
                "style": "form",
                "explode": true
            }
            """,
            null,
            true,
            null
        }
    };

    public static TheoryData<string, string, bool, string?> StringLabel => new()
    {
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "string" 
                },
                "style": "label",
                "explode": true
            }
            """,
            ".test",
            true,
            "\"test\""
        }
    };

    public static TheoryData<string, string, bool, string?> NumberLabel => new()
    {
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "number" 
                },
                "style": "label",
                "explode": true
            }
            """,
            ".1.2",
            true,
            "1.2"
        }
    };

    public static TheoryData<string, string, bool, string?> IntegerLabel => new()
    {
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "integer" 
                },
                "style": "label",
                "explode": true
            }
            """,
            ".1",
            true,
            "1"
        }
    };

    public static TheoryData<string, string, bool, string?> BooleanLabel => new()
    {
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "boolean" 
                },
                "style": "label",
                "explode": true}
            """,
            ".true",
            true,
            "true"
        }
    };

    public static TheoryData<string, string?, bool, string?> NullLabel => new()
    {
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "null" 
                },
                "style": "label",
                "explode": true
            }
            """,
            ".",
            true,
            null
        },
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "label",
                "explode": true
            }
            """,
            "",
            true,
            null
        },
        {
            """
            {
                "in": "path",
                "schema": {
                    "type": "null"
                },
                "style": "label",
                "explode": true
            }
            """,
            null,
            true,
            null
        }
    };


    public static TheoryData<string, string?, bool, string?> EmptySchema => new()
    {
        {
            """
            {
                "in": "query",
                "schema": {
                },
                "style": "form",
                "explode": true
            }
            """,
            "color=test",
            true,
            """["test"]"""
        },
        {
            """
            {
                "in": "query",
                "schema": {
                },
                "style": "form",
                "explode": true
            }
            """,
            "color=test&color=test2",
            true,
            """["test","test2"]"""
        },
        {
            """
            {
                "in": "path",
                "schema": {
                },
                "style": "simple",
                "explode": false
            }
            """,
            "",
            true,
            """[""]"""
        },
        {
            """
            {
                "in": "path",
                "schema": {
                },
                "style": "label",
                "explode": false
            }
            """,
            ".",
            true,
            """[""]"""
        }
    };
}
