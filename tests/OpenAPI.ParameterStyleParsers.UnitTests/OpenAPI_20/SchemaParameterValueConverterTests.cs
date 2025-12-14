using System.Text.Json.Nodes;
using AwesomeAssertions;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers;
using OpenAPI.ParameterStyleParsers.UnitTests.Xunit;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_20;

public class SchemaParameterValueConverterTests
{
    [Theory]
    [MemberData(nameof(String))]
    [MemberData(nameof(Number))]
    [MemberData(nameof(Integer))]
    [MemberData(nameof(Boolean))]
    public void Given_a_simple_primitive_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(String))]
    [MemberData(nameof(Number))]
    [MemberData(nameof(Integer))]
    [MemberData(nameof(Boolean))]
    public void Given_a_simple_primitive_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
        string parameterJson,
        string?[] value,
        bool shouldMap,
        string? jsonInstance)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(ArrayMulti))]
    [MemberData(nameof(ArrayCommaSeparated))]
    [MemberData(nameof(ArraySpaceSeparated))]
    [MemberData(nameof(ArrayPipeSeparated))]
    [MemberData(nameof(ArrayTabSeparated))]
    public void Given_an_array_parameter_with_schema_When_mapping_values_It_should_map_the_value_to_proper_json(
        string parameterJson,
        string?[] values,
        bool shouldMap,
        string? jsonInstance)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance);
    }

    [Theory]
    [MemberData(nameof(ArrayMulti))]
    [MemberData(nameof(ArrayCommaSeparated))]
    [MemberData(nameof(ArraySpaceSeparated))]
    [MemberData(nameof(ArrayPipeSeparated))]
    [MemberData(nameof(ArrayTabSeparated))]
    public void Given_an_array_parameter_with_schema_When_serializing_It_should_serialize_the_json_instance(
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
        var parameter = OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterJson);
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

    private static ParameterValueParser CreateParameterValueParser(
        string parameterJson)
    {
        var parameter = OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        var parser = ParameterValueParser.Create(parameter);
        return parser;
    }

    #region Array
    public static readonly TheoryData<string, string?[], bool, string?> ArrayMulti = new()
    {
        {
            """
            {
                "name": "color",
                "in": "query",
                "type": "array",
                "collectionFormat": "multi",
                "items": {
                    "type": "string"
                }
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
                "in": "formData",
                "type": "array",
                "collectionFormat": "multi",
                "items": {
                    "type": "string"
                }
            }
            """,
            ["color=test&color=test2"],
            true,
            "[\"test\",\"test2\"]"
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?> ArrayCommaSeparated =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.Path))
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.FormData))
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.Header))
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.Query));
    public static readonly TheoryData<string, string?[], bool, string?> ArrayPipeSeparated =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.Path))
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.FormData))
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.Header))
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.Query));
    public static readonly TheoryData<string, string?[], bool, string?> ArraySpaceSeparated =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.Path))
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.FormData))
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.Header))
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.Query));
    public static readonly TheoryData<string, string?[], bool, string?> ArrayTabSeparated =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(CreateArrayTabSeparated(OpenApi20.Parameter.Locations.Path))
            .AddRange(CreateArrayTabSeparated(OpenApi20.Parameter.Locations.FormData))
            .AddRange(CreateArrayTabSeparated(OpenApi20.Parameter.Locations.Header))
            .AddRange(CreateArrayTabSeparated(OpenApi20.Parameter.Locations.Query));

    private static Dictionary<char, string> DelimiterToCollectionFormatMap => new()
    {
        [' '] = "ssv",
        [','] = "csv",
        ['\t'] = "tsv",
        ['|'] = "pipes"
    };

    private static Dictionary<string, bool> LocationToHasKeyMap => new()
    {
        ["path"] = false,
        ["header"] = false,
        ["query"] = true,
        ["formData"] = true
    };

    private static string[] AllParameterLocations =>
    [
        OpenApi20.Parameter.Locations.Path,
        OpenApi20.Parameter.Locations.FormData, 
        OpenApi20.Parameter.Locations.Header,
        OpenApi20.Parameter.Locations.Query
    ]; 
    
    private static (string, string?[], bool, string?)[] CreateArrayCommaSeparated(string @in) => CreateArray(@in, ',');
    private static (string, string?[], bool, string?)[] CreateArraySpaceSeparated(string @in) => CreateArray(@in, ' ');
    private static (string, string?[], bool, string?)[] CreateArrayPipeSeparated(string @in) => CreateArray(@in, '|');
    private static (string, string?[], bool, string?)[] CreateArrayTabSeparated(string @in) => CreateArray(@in, '\t');
    private static (string, string?[], bool, string?)[] CreateArray(string @in, char delimiter)
    {
        var hasKey = LocationToHasKeyMap[@in];
        var key = hasKey ? "color=" : "";
        var collectionFormat = DelimiterToCollectionFormatMap[delimiter];
        var schema = $$"""
                       {
                           "name": "color",
                           "in": "{{@in}}",
                           "type": "array",
                           "collectionFormat": "{{collectionFormat}}",
                           "items": {
                               "type": "string"
                           }
                       }
                       """;
        return
        [
            (
                schema,
                [$"{key}test{delimiter}test2"],
                true,
                "[\"test\",\"test2\"]"
            ),
            (
                schema,
                [$"{key}test{delimiter}"],
                true,
                "[\"test\",\"\"]"
            ),
            (
                schema,
                [$"{key}test{delimiter}test2"],
                true,
                "[\"test\",\"test2\"]"
            ),
            (
                schema,
                [$"{key}test{delimiter}"],
                true,
                "[\"test\",\"\"]"
            )
        ];
    }
    #endregion

    public static readonly TheoryData<string, string?[], bool, string?> String =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(
                CreateTestScenarios("string", AllParameterLocations, ["\"1.2\"", "\"test\""]));

    public static readonly TheoryData<string, string?[], bool, string?> Number =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(
                CreateTestScenarios("number", AllParameterLocations, ["1.2", "\"foo\""]));

    public static readonly TheoryData<string, string?[], bool, string?> Integer =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(
                CreateTestScenarios("integer", AllParameterLocations, ["1", "false", "\"foo\""]));

    public static readonly TheoryData<string, string?[], bool, string?> Boolean =
        new TheoryData<string, string?[], bool, string?>()
            .AddRange(
                CreateTestScenarios("boolean", AllParameterLocations, ["true", "\"foo\""]));
    
    private static (string, string?[], bool, string?)[] CreateTestScenarios(string type, string[] ins,
        string[] jsonValues) =>
        ins.SelectMany(@in =>
                jsonValues.Select(val =>
                    CreateValue(@in, type, val)))
            .ToArray();

    private static (string, string?[], bool, string?) CreateValue(string @in, string type, string jsonValue)
    {
        var hasKey = LocationToHasKeyMap[@in];
        var key = hasKey ? "color=" : "";
        return ($$"""
                  {
                      "name": "color",
                      "in": "{{@in}}",
                      "type": "{{type}}"
                  }
                  """, [$"{key}{jsonValue.Trim('"')}"], true, jsonValue);
    }
}
