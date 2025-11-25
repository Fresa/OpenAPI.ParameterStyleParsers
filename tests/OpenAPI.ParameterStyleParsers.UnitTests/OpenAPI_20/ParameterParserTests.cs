using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_20;

public class ParameterParserTests
{
    [Theory]
    [InlineData("header", "string")]
    [InlineData("header", "integer")]
    [InlineData("header", "number")]
    [InlineData("header", "boolean")]
    [InlineData("body", null)]
    [InlineData("header", "file")]
    public void Given_valid_primitive_parameter_input_When_parsing_The_parameter_should_be_parsed(
        string @in,
        string? type)
    {
       var parameter = OpenApi20.Parameter.Parse(
            name: "test",
            @in: @in,
            type: type);
       parameter.Name.Should().Be("test");
       parameter.Type.Should().Be(type);
       parameter.IsArray.Should().Be(false);
       AssertLocations(parameter, @in);
    }

    [Theory]
    [InlineData("header", "StRiNg")]
    [InlineData("notheader", "string")]
    [InlineData("header", null)]
    public void Given_invalid_primitive_parameter_input_When_parsing_The_parameter_should_fail_parsing(
        string @in,
        string? type)
    {
        Action parse = () => OpenApi20.Parameter.Parse(
            name: "test",
            @in: @in,
            type: type);
        
        parse.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [InlineData("header", "csv", "string")]
    [InlineData("query", "ssv", "string")]
    [InlineData("header", "tsv", "string")]
    [InlineData("header", "pipes", "string")]
    [InlineData("query", "multi", "string")]
    [InlineData("formData", "multi", "string")]
    public void Given_array_parameter_input_When_parsing_The_parameter_should_be_parsed(
        string @in,
        string collectionFormat,
        string itemsObjectType)
    {
        var parameter = OpenApi20.Parameter.Parse(
            name: "test",
            @in: @in,
            type: OpenApi20.Parameter.Types.Array,
            collectionFormat: collectionFormat,
            items: ItemsObject.Parse(itemsObjectType));
        AssertLocations(parameter, @in);
    }

    [Theory]
    [InlineData("header", "foo", "string")]
    [InlineData("header", "csv", "int32")]
    [InlineData("header", "csv", null)]
    [InlineData("body", "csv", "string")]
    [InlineData("header", "multi", "string")]
    [InlineData("path", "multi", "string")]
    [InlineData("body", "multi", "string")]
    public void Given_invalid_array_parameter_input_When_parsing_The_parameter_should_fail_parsing(
        string @in,
        string collectionFormat,
        string itemsObjectType)
    {
        Action parse = () => OpenApi20.Parameter.Parse(
            name: "test",
            @in: @in,
            type: OpenApi20.Parameter.Types.Array,
            collectionFormat: collectionFormat,
            items: ItemsObject.Parse(itemsObjectType));
        
        parse.Should().Throw<InvalidOperationException>();
    }
    
    private static void AssertLocations(OpenApi20.Parameter parameter, string @in)
    {
        parameter.InBody.Should().Be(@in == OpenApi20.Parameter.Locations.Body);
        parameter.InQuery.Should().Be(@in == OpenApi20.Parameter.Locations.Query);
        parameter.InHeader.Should().Be(@in == OpenApi20.Parameter.Locations.Header);
        parameter.InFormData.Should().Be(@in == OpenApi20.Parameter.Locations.FormData);
        parameter.InPath.Should().Be(@in == OpenApi20.Parameter.Locations.Path);
    }
}
