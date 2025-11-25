using OpenAPI.ParameterStyleParsers.Json;
using OpenAPI.ParameterStyleParsers.UnitTests.Xunit;
using Socolin.TestUtils.JsonComparer;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_20;

public class GenerateParameterSchemaTests
{
    [Fact]
    public void Given_a_body_parameter_When_generating_schema_It_should_get_the_schema_property()
    {
        const string parameterJson =
            """
            {
                "name": "test",
                "in": "body",
                "schema": {
                    "type": "string"
                }
            }
            """;
        var jsonSchema = OpenApi20.Parameter.GetSchema(parameterJson);
        jsonSchema.Should().NotBeNull();
        jsonSchema.AsObject();
        var schema = jsonSchema.AsObject();
        schema.Count.Should().Be(1);
        schema.GetRequiredPropertyValue<string>("type").Should().Be("string");
    }

    [Fact]
    public void Given_a_file_type_parameter_When_generating_the_schema_It_should_return_no_schema()
    {
        const string parameterJson =
            """
            {
                "name": "test",
                "in": "formData",
                "type": "file",
                "schema": {
                    "type": "string"
                }
            }
            """;
        var jsonSchema = OpenApi20.Parameter.GetSchema(parameterJson);
        jsonSchema.Should().BeNull();
    }
    
    [Theory]
    [MemberData(nameof(Parameters))]
    public void Given_a_parameter_When_generating_the_schema_It_should_generate_schema_according_to_properties(
        string testCase,
        string parameterJson,
        string expectedSchema)
    {
        var jsonSchema = OpenApi20.Parameter.GetSchema(parameterJson);
        jsonSchema.Should().NotBeNull();
        var schema = jsonSchema.ToJsonString();
        var errors = JsonComparer.GetDefault().Compare(schema, expectedSchema);
        errors.Should().HaveCount(0, $"{testCase}: {JsonComparerOutputFormatter.GetReadableMessage(schema, expectedSchema, errors)}");
    }

    public static readonly TheoryData<string, string, string> Parameters = new TheoryData<string, string, string>()
        .AddRange(CreateParameters());
    private static (string, string, string)[] CreateParameters()
    {
        (string, string)[] scenarios =
        [
            (
                """
                {
                    "name": "color",
                    "description": "favorite colors",
                    "in": "query",
                    "type": "array",
                    "items": {
                        "type": "string"
                    },
                    "uniqueItems": true,
                    "minItems": 1,
                    "maxItems": 5,
                    "x-mode": "basic",
                    "y-mode": "basic"
                }
                """,
                """
                {
                    "type": "array",
                    "description": "favorite colors",
                    "items": {
                        "type": "string"
                    },
                    "uniqueItems": true,
                    "minItems": 1,
                    "maxItems": 5,
                    "x-mode": "basic"
                }
                """
            ),
            (
                """
                {
                    "name": "color",
                    "in": "query",
                    "type": "array",
                    "items": {
                        "type": "array",
                        "items": {
                            "type": "string"
                        }
                    }
                }
                """,
                """
                {
                    "type": "array",
                    "items": {
                        "type": "array",
                        "items": {
                            "type": "string"
                        }
                    }
                }
                """
            ),
            (
                """
                {
                    "name": "color",
                    "in": "query",
                    "type": "string",
                    "pattern": "^[A-Z]$",
                    "enum": ["YELLOW"],
                    "maxLength": 20,
                    "minLength": 1
                }
                """,
                """
                {
                    "type": "string",
                    "pattern": "^[A-Z]$",
                    "enum": [
                        "YELLOW"
                    ],
                    "maxLength": 20,
                    "minLength": 1
                }
                """
            ),
            (
                """
                {
                    "name": "color",
                    "in": "query",
                    "type": "number",
                    "default": 0,
                    "maximum": 10,
                    "exclusiveMaximum": false,
                    "minimum": 0,
                    "exclusiveMinimum": false,
                    "multipleOf": 2
                    
                }
                """,
                """
                {
                    "type": "number",
                    "default": 0,
                    "maximum": 10,
                    "exclusiveMaximum": false,
                    "minimum": 0,
                    "exclusiveMinimum": false,
                    "multipleOf": 2
                }
                """
            )
        ];

        return scenarios.Select((tuple, i) => ($"{nameof(CreateParameters)}[{i}]", tuple.Item1, tuple.Item2)).ToArray();
    }
}