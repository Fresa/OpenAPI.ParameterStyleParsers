using System.Text.Json.Nodes;
using FluentAssertions;
using OpenAPI.ParameterStyleParsers.Json;
using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers;

namespace OpenAPI.ParameterStyleParsers.UnitTests;

public class JsonSchema202012_InstanceTypeTests
{
    [Theory]
    [MemberData(nameof(InstanceTypes))]
    public void Given_a_json_schema_when_parsing_It_should_parse_type(
        InstanceType? expectedInstanceTypes,
        string pointer,
        string schemaAsJson)
    {
        var jsonSchemaNode = JsonNode
            .Parse(schemaAsJson)
            .Resolve(JsonPointer.Parse(pointer));
        var schema = new JsonSchema202012(jsonSchemaNode);
        var instanceType = schema.GetInstanceType();
        if (expectedInstanceTypes is null)
        {
            instanceType.Should().BeNull();
            return;
        }

        instanceType.Should().NotBeNull();
        instanceType.Should().Be(expectedInstanceTypes);
    }

    [Theory]
    [MemberData(nameof(Properties))]
    public void Given_a_json_schema_when_parsing_It_should_parse_properties(
        string pointer,
        string schemaAsJson,
        string[]? expectedPropertyNames)
    {
        var jsonSchemaNode = JsonNode
            .Parse(schemaAsJson)
            .Resolve(JsonPointer.Parse(pointer));
        var schema = new JsonSchema202012(jsonSchemaNode);
        var properties = schema.GetProperties();
        if (expectedPropertyNames is null)
        {
            properties.Should().BeNull();
            return;
        }

        properties.Should().NotBeNull();
        properties.Should().ContainKeys(expectedPropertyNames);
        properties!.Values.Should().AllSatisfy(propertySchema =>
            propertySchema.GetInstanceType()
                .Should().Be(InstanceType.String));
    }

    [Theory]
    [MemberData(nameof(Items))]
    public void Given_a_json_schema_when_parsing_It_should_parse_items(
        string pointer,
        string schemaAsJson,
        InstanceType? expectedInstanceTypes)
    {
        var jsonSchemaNode = JsonNode
            .Parse(schemaAsJson)
            .Resolve(JsonPointer.Parse(pointer));
        var schema = new JsonSchema202012(jsonSchemaNode);
        var items = schema.GetItems();
        if (expectedInstanceTypes is null)
        {
            items?.GetInstanceType().Should().BeNull();
            return;
        }

        items.Should().NotBeNull();
        var instanceType = items!.GetInstanceType();
        instanceType.Should().NotBeNull();
        instanceType.Should().Be(expectedInstanceTypes);
    }

    [Theory]
    [MemberData(nameof(AdditionalProperties))]
    public void Given_a_json_schema_when_parsing_It_should_parse_additional_properties(
        string pointer,
        string schemaAsJson,
        InstanceType? expectedInstanceType)
    {
        var jsonSchemaNode = JsonNode
            .Parse(schemaAsJson)
            .Resolve(JsonPointer.Parse(pointer));
        var schema = new JsonSchema202012(jsonSchemaNode);
        var items = schema.GetAdditionalProperties();
        if (expectedInstanceType is null)
        {
            items?.GetInstanceType().Should().BeNull();
            return;
        }

        items.Should().NotBeNull();
        var instanceType = items!.GetInstanceType();
        instanceType.Should().NotBeNull();
        instanceType.Should().Be(expectedInstanceType);
    }

    public static readonly TheoryData<InstanceType?, string, string> InstanceTypes = new()
    {
        {
            InstanceType.String,
            "#/schema",
            """
            {
                "schema": {
                    "type": "string"
                }
            }
            """
        },
        {
            InstanceType.String | InstanceType.Null,
            "#/schema",
            """
            {
                "schema": {
                    "type": ["string", "null"]
                }
            }
            """
        },
        {
            InstanceType.Integer,
            "#",
            """
            {
                "type": "integer"
            }
            """
        },
        {
            InstanceType.Array,
            "#",
            """
            {
                "type": "array"
            }
            """
        },
        {
            InstanceType.Object,
            "#",
            """
            {
                "type": "object"
            }
            """
        },
        {
            InstanceType.Boolean,
            "#",
            """
            {
                "type": "boolean"
            }
            """
        },
        {
            InstanceType.Number,
            "#",
            """
            {
                "type": "number"
            }
            """
        },
        {
            InstanceType.Null,
            "#",
            """
            {
                "type": "null"
            }
            """
        },
        {
            null,
            "#",
            """
            {
            }
            """
        }
    };

    public static readonly TheoryData<string, string, string[]?> Properties = new()
    {
        {
            "#/schema",
            """
            {
                "schema": {
                    "properties": {
                        "foo": {
                            "type": "string"
                        },
                        "bar": {
                            "type": "string"
                        }
                    }
                }
            }
            """,
            ["foo", "bar"]
        },
        {
            "#",
            """
            {
            }
            """,
            null
        }
    };

    public static readonly TheoryData<string, string, InstanceType?> AdditionalProperties = new()
    {
        {
            "#/schema",
            """
            {
                "schema": {
                    "additionalProperties": {
                        "type": "string"
                    }
                }
            }
            """,
            InstanceType.String
        },
        {
            "#",
            """
            {
                "additionalProperties": {
                }
            }
            """,
            null
        },
        {
            "#",
            """
            {
                "additionalProperties": false
            }
            """,
            null
        }
    };

    public static readonly TheoryData<string, string, InstanceType?> Items = new()
    {
        {
            "#/schema",
            """
            {
                "schema": {
                    "items": {
                        "type": "string"
                    }
                }
            }
            """,
            InstanceType.String
        },
        {
            "#",
            """
            {
                "items": {}
            }
            """,
            null
        },
        {
            "#",
            """
            {
            }
            """,
            null
        }
    };
}
