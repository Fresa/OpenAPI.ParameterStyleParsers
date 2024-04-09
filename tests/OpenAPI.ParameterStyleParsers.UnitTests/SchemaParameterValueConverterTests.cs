using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Json.Pointer;
using Json.Schema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Array;

namespace OpenAPI.ParameterStyleParsers.UnitTests;

public class SchemaParameterValueConverterTests
{
    [Theory]
    [MemberData(nameof(String))]
    [MemberData(nameof(Number))]
    [MemberData(nameof(Integer))]
    [MemberData(nameof(Boolean))]
    [MemberData(nameof(Null))]
    [MemberData(nameof(EmptySchema))]
    [MemberData(nameof(ArrayLabel))]
    [MemberData(nameof(ArrayForm))]
    [MemberData(nameof(ArrayMatrix))]
    [MemberData(nameof(ArraySimple))]
    [MemberData(nameof(ArraySpaceDelimited))]
    [MemberData(nameof(ArrayPipeDelimited))]
    [MemberData(nameof(ObjectForm))]
    [MemberData(nameof(ObjectMatrix))]
    [MemberData(nameof(ObjectLabel))]
    [MemberData(nameof(DeepObject))]
    public void Given_a_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        var parameterJsonNode = JsonNode.Parse(parameterJson)!;
        var reader = new JsonNodeReader(parameterJsonNode, JsonPointer.Empty);
        var schema = parameterJsonNode["schema"].Deserialize<JsonSchema>();
        var parameter =
            Parameter.Parse(
                reader.Read("style").GetValue<string>(), 
                reader.Read("explode").GetValue<bool>());
        var parser = ParameterValueParser.Create(parameter, schema!);
        parser.TryParse(values, out var instance, out var mappingError).Should().Be(shouldMap, mappingError);
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
            jsonInstance.Should().NotBeNullOrEmpty();
            instance.ToJsonString().Should().BeEquivalentTo(jsonInstance);
        }
    }

    #region Object
    public static TheoryData<string, string[], bool, string?> DeepObject => new()
    {
        {
            """
            {
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
            new[] { "color[R]=100", "color[G]=200", "color[B]=150" },
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            new[] { "color[R]=100", "color[G]=200", "color[B]=" },
            true,
            """{"R":"100","G":"200","B":""}"""
        }
    };
    public static TheoryData<string, string[], bool, string?> ObjectLabel => new()
    {
        {
            """
            {
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
            new[] { ".R=100.G=200.B=150" },
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            new[] { ".R=100.G=200.B" },
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            new[] { ".R.100.G.200.B.150" },
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
        """
            {
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
        new[] { ".R.100.G.200.B." },
        true,
        """{"R":"100","G":"200","B":""}"""
        }
    };
    public static TheoryData<string, string[], bool, string?> ObjectMatrix => new()
    {
        {
            """
            {
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
            new[] { ";R=100;G=200;B=150" },
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            [";R=100;G=200;B"],
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            [";color=R,100,G,200,B,150"],
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            [";keys=comma,%2C,dot,.,semi,%3B"],
            true,
            """{"comma":",","dot":".","semi":";"}"""
        }
    };
    public static TheoryData<string, string[], bool, string?> ObjectForm => new()
    {
        {
            """
            {
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
            new[] { "R,100,G,200,B,150" },
            true,
            """{"R":100,"G":200,"B":150}"""
        },
        {
            """
            {
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
            new[] { "R,100,G,200,B," },
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            new[] { "R,100,G,200,B," },
            true,
            """{"R":"100","G":"200","B":""}"""
        },
        {
            """
            {
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
            new[] { "R,100,G,200,B," },
            true,
            """{"R":100,"G":200,"B":""}"""
        }
    };
    #endregion

    #region Array
    public static TheoryData<string, string[], bool, string?> ArrayLabel => new()
    {
        {
            """
            {
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
            new[] { ".test.test2" },
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            new[] { ".test.test2" },
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            new[] { ".test." },
            true,
            "[\"test\",\"\"]"
        }
    };

    public static TheoryData<string, string[], bool, string?> ArrayForm => new()
    {
        {
            """
            {
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
            new[] { "test", "test2" },
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            new[] { "test,test2" },
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static TheoryData<string, string[], bool, string?> ArrayMatrix => new()
    {
        {
            """
            {
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
            new[] { ";test=test;test=test2" },
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            new[] { ";test=test;test" },
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            new[] { ";test=test,test2" },
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static TheoryData<string, string[], bool, string?> ArraySimple => new()
    {
        {
            """
            {
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
            new[] { "test,test2" },
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            new[] { "test," },
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            new[] { "test,test2" },
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static TheoryData<string, string[], bool, string?> ArraySpaceDelimited => new()
    {
        {
            """
            {
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
            new[] { "test test2" },
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            new[] { "test " },
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            new[] { "test", "test2" },
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static TheoryData<string, string[], bool, string?> ArrayPipeDelimited => new()
    {
        {
            """
            {
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
            new[] { "test|test2" },
            true,
            "[\"test\",\"test2\"]"
        },
        {
            """
            {
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
            new[] { "test|" },
            true,
            "[\"test\",\"\"]"
        },
        {
            """
            {
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
            new[] { "test", "test2" },
            true,
            "[\"test\",\"test2\"]"
        }
    };
    #endregion

    public static TheoryData<string, string[], bool, string?> String => new()
    {
        {
            """
            {
                "schema": {
                    "type": "string" 
                },
                "style": "form",
                "explode": true
            }
            """,
            new[] { "test" },
            true,
            "\"test\""
        },
        {
            """
            {
                "schema": {
                    "type": "string" 
                },
                "style": "form",
                "explode": true
            }
            """,
            new[] { "test", "test2" },
            false,
            null
        }
    };

    public static TheoryData<string, string[], bool, string?> Number => new()
    {
        {
            """
            {
                "schema": {
                    "type": "number" 
                },
                "style": "form",
                "explode": true
            }
            """,
            new[] { "1.2" },
            true,
            "1.2"
        },
        {
            """
            {
                "schema": {
                    "type": "number" 
                },
                "style": "form",
                "explode": true
            }
            """,
            new[] { "1.2", "1.3" },
            false,
            null
        }
    };

    public static TheoryData<string, string[], bool, string?> Integer => new()
    {
        {
            """
            {
                "schema": {
                    "type": "integer" 
                },
                "style": "form",
                "explode": true
            }
            """,
            new[] { "1" },
            true,
            "1"
        }
    };

    public static TheoryData<string, string[], bool, string?> Boolean => new()
    {
        {
            """
            {
                "schema": {
                    "type": "boolean" 
                },
                "style": "form",
                "explode": true}
            """,
            new[] { "true" },
            true,
            "true"
        }
    };

    public static TheoryData<string, string[], bool, string?> Null => new()
    {
        {
            """
            {
                "schema": {
                    "type": "null" 
                },
                "style": "form",
                "explode": true
            }
            """,
            System.Array.Empty<string>(),
            true,
            null
        }
    };

    public static TheoryData<string, string[], bool, string?> EmptySchema => new()
    {
        {
            """
            {
                "schema": {
                },
                "style": "form",
                "explode": true
            }
            """,
            new[] { "test" },
            true,
            "\"test\""
        },
        {
            """
            {
                "schema": {
                },
                "style": "form",
                "explode": true
            }
            """,
            new[] { "test", "test2" },
            true,
            """["test","test2"]"""
        },
        {
            """
            {
                "schema": {
                },
                "style": "simple",
                "explode": false
            }
            """,
            Array.Empty<string>(),
            true,
            null
        }
    };
}
