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
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
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
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestSerializing(parameterJson, value, shouldMap, jsonInstance, expectedValueIncludesParameterName);
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
        string? jsonInstance,
        bool expectedValueIncludesParameterName)
    {
        TestParsing(parameterJson, values, shouldMap, jsonInstance, expectedValueIncludesParameterName);
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
        var parameter = OpenApi20.Parameter.FromOpenApi20ParameterSpecification(parameterJson);
        parameter.Should().NotBeNull();
        var parser = ParameterValueParser.Create(parameter);
        parser.TryParse(value, out var instance, out var mappingError).Should().Be(shouldMap, mappingError);
        parser.ValueIncludesParameterName.Should().Be(expectedValueIncludesParameterName);
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
        var serialized = parser.Serialize(jsonInstance == null ? null : JsonNode.Parse(jsonInstance));

        expectedValues.Should().Contain(serialized);
        parser.ValueIncludesParameterName.Should().Be(expectedValueIncludesParameterName);
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
    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayMulti = new()
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
            "[\"test\",\"test2\"]",
            true
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
            "[\"test\",\"test2\"]",
            true
        }
    };

    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayCommaSeparated =
        new TheoryData<string, string?[], bool, string?, bool>()
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.Path))
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.FormData))
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.Header))
            .AddRange(CreateArrayCommaSeparated(OpenApi20.Parameter.Locations.Query));
    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayPipeSeparated =
        new TheoryData<string, string?[], bool, string?, bool>()
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.Path))
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.FormData))
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.Header))
            .AddRange(CreateArrayPipeSeparated(OpenApi20.Parameter.Locations.Query));
    public static readonly TheoryData<string, string?[], bool, string?, bool> ArraySpaceSeparated =
        new TheoryData<string, string?[], bool, string?, bool>()
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.Path))
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.FormData))
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.Header))
            .AddRange(CreateArraySpaceSeparated(OpenApi20.Parameter.Locations.Query));
    public static readonly TheoryData<string, string?[], bool, string?, bool> ArrayTabSeparated =
        new TheoryData<string, string?[], bool, string?, bool>()
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
    
    private static (string, string?[], bool, string?, bool)[] CreateArrayCommaSeparated(string @in) => CreateArray(@in, ',');
    private static (string, string?[], bool, string?, bool)[] CreateArraySpaceSeparated(string @in) => CreateArray(@in, ' ');
    private static (string, string?[], bool, string?, bool)[] CreateArrayPipeSeparated(string @in) => CreateArray(@in, '|');
    private static (string, string?[], bool, string?, bool)[] CreateArrayTabSeparated(string @in) => CreateArray(@in, '\t');
    private static (string, string?[], bool, string?, bool)[] CreateArray(string @in, char delimiter)
    {
        var hasKey = LocationToHasKeyMap[@in];
        var key = hasKey ? "color=" : "";
        var collectionFormat = DelimiterToCollectionFormatMap[delimiter];
        var valueIncludesParameterName = hasKey;
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
                "[\"test\",\"test2\"]",
                valueIncludesParameterName
            ),
            (
                schema,
                [$"{key}test{delimiter}"],
                true,
                "[\"test\",\"\"]",
                valueIncludesParameterName
            ),
            (
                schema,
                [$"{key}test{delimiter}test2"],
                true,
                "[\"test\",\"test2\"]",
                valueIncludesParameterName
            ),
            (
                schema,
                [$"{key}test{delimiter}"],
                true,
                "[\"test\",\"\"]",
                valueIncludesParameterName
            )
        ];
    }
    #endregion

    public static readonly TheoryData<string, string?[], bool, string?, bool> String =
        new TheoryData<string, string?[], bool, string?, bool>()
            .AddRange(
                CreateTestScenarios("string", AllParameterLocations, ["\"1.2\"", "\"test\""]));

    public static readonly TheoryData<string, string?[], bool, string?, bool> Number =
        new TheoryData<string, string?[], bool, string?, bool>()
            .AddRange(
                CreateTestScenarios("number", AllParameterLocations, ["1.2", "\"foo\""]));

    public static readonly TheoryData<string, string?[], bool, string?, bool> Integer =
        new TheoryData<string, string?[], bool, string?, bool>()
            .AddRange(
                CreateTestScenarios("integer", AllParameterLocations, ["1", "false", "\"foo\""]));

    public static readonly TheoryData<string, string?[], bool, string?, bool> Boolean =
        new TheoryData<string, string?[], bool, string?, bool>()
            .AddRange(
                CreateTestScenarios("boolean", AllParameterLocations, ["true", "\"foo\""]));

    private static (string, string?[], bool, string?, bool)[] CreateTestScenarios(string type, string[] ins,
        string[] jsonValues) =>
        ins.SelectMany(@in =>
                jsonValues.Select(val =>
                    CreateValue(@in, type, val)))
            .ToArray();

    private static (string, string?[], bool, string?, bool) CreateValue(string @in, string type, string jsonValue)
    {
        var hasKey = LocationToHasKeyMap[@in];
        var key = hasKey ? "color=" : "";
        var valueIncludesParameterName = hasKey;
        return ($$"""
                  {
                      "name": "color",
                      "in": "{{@in}}",
                      "type": "{{type}}"
                  }
                  """, [$"{key}{jsonValue.Trim('"')}"], true, jsonValue, valueIncludesParameterName);
    }
}
