using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenApi32 = OpenAPI.ParameterStyleParsers.OpenApi32;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_32;

public class ParameterParserTests
{
    [Theory]
    [InlineData("path", "simple")]
    [InlineData("path", "label")]
    [InlineData("path", "matrix")]
    [InlineData("header", "simple")]
    [InlineData("query", "form")]
    [InlineData("query", "spaceDelimited")]
    [InlineData("query", "pipeDelimited")]
    [InlineData("query", "deepObject")]
    [InlineData("cookie", "form")]
    [InlineData("cookie", "cookie")]
    public void Given_valid_parameter_input_When_parsing_The_parameter_should_be_parsed(
        string @in,
        string style)
    {
        var schema = new JsonSchema202012("""{"type": "string"}""");
        var parameter = OpenApi32.Parameter.Parse(
            name: "test",
            style: style,
            location: @in,
            explode: false,
            jsonSchema: schema);

        parameter.Name.Should().Be("test");
        parameter.Style.Should().Be(style);
        parameter.Location.Should().Be(@in);
        parameter.Explode.Should().Be(false);
    }

    [Theory]
    [InlineData("path", "form")]
    [InlineData("path", "deepObject")]
    [InlineData("header", "matrix")]
    [InlineData("header", "form")]
    [InlineData("query", "simple")]
    [InlineData("query", "matrix")]
    [InlineData("cookie", "simple")]
    [InlineData("cookie", "matrix")]
    [InlineData("invalid", "simple")]
    public void Given_invalid_parameter_input_When_parsing_The_parameter_should_fail_parsing(
        string @in,
        string style)
    {
        var schema = new JsonSchema202012("""{"type": "string"}""");
        Action parse = () => OpenApi32.Parameter.Parse(
            name: "test",
            style: style,
            location: @in,
            explode: false,
            jsonSchema: schema);

        parse.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData("path", "simple", true)]
    [InlineData("path", "simple", false)]
    [InlineData("query", "form", true)]
    [InlineData("query", "form", false)]
    [InlineData("cookie", "cookie", true)]
    [InlineData("cookie", "cookie", false)]
    public void Given_valid_array_parameter_input_When_parsing_The_parameter_should_be_parsed(
        string @in,
        string style,
        bool explode)
    {
        var schema = new JsonSchema202012("""{"type": "array", "items": {"type": "string"}}""");
        var parameter = OpenApi32.Parameter.Parse(
            name: "test",
            style: style,
            location: @in,
            explode: explode,
            jsonSchema: schema);

        parameter.Name.Should().Be("test");
        parameter.Style.Should().Be(style);
        parameter.Location.Should().Be(@in);
        parameter.Explode.Should().Be(explode);
    }

    [Theory]
    [InlineData("path", "simple", true)]
    [InlineData("path", "simple", false)]
    [InlineData("query", "form", true)]
    [InlineData("query", "form", false)]
    [InlineData("query", "deepObject", true)]
    [InlineData("cookie", "cookie", true)]
    [InlineData("cookie", "cookie", false)]
    public void Given_valid_object_parameter_input_When_parsing_The_parameter_should_be_parsed(
        string @in,
        string style,
        bool explode)
    {
        var schema = new JsonSchema202012("""{"type": "object", "properties": {"foo": {"type": "string"}}}""");
        var parameter = OpenApi32.Parameter.Parse(
            name: "test",
            style: style,
            location: @in,
            explode: explode,
            jsonSchema: schema);

        parameter.Name.Should().Be("test");
        parameter.Style.Should().Be(style);
        parameter.Location.Should().Be(@in);
        parameter.Explode.Should().Be(explode);
    }

    [Theory]
    [InlineData("path", "simple", false)]
    [InlineData("header", "simple", false)]
    [InlineData("query", "form", true)]
    [InlineData("cookie", "form", true)]
    public void Given_parameter_without_style_When_parsing_The_default_style_should_be_used(
        string @in,
        string expectedStyle,
        bool expectedExplode)
    {
        var parameterJson = $$"""
            {
                "name": "test",
                "in": "{{@in}}",
                "schema": { "type": "string" }
            }
            """;

        var parameter = OpenApi32.Parameter.FromOpenApi32ParameterSpecification(parameterJson);

        parameter.Style.Should().Be(expectedStyle);
        parameter.Explode.Should().Be(expectedExplode);
    }
}