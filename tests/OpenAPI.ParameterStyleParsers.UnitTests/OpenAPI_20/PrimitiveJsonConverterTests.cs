using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_20;

public class PrimitiveJsonConverterTests
{
    [Theory]
    [InlineData("integer", "one")]
    [InlineData("number", "one")]
    [InlineData("boolean", "one")]
    public void InvalidTypes_Converting_ReportError(string type, string value)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeFalse();
        instance.Should().BeNull();
        error.Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [InlineData("integer", "1")]
    [InlineData("integer", "1.0")]
    [InlineData("integer", "-0")]
    [InlineData("integer",  "-9007199254740991")]
    [InlineData("integer",  "9007199254740992")]
    [InlineData("number", "1")]
    [InlineData("number", "1.0")]
    [InlineData("number", "-0")]
    [InlineData("number",  "-9007199254740991")]
    [InlineData("number",  "9007199254740992")]
    [InlineData("number",  "1.5")]
    [InlineData("boolean",  "true")]
    [InlineData("boolean",  "false")]
    [InlineData("string",  "false")]
    [InlineData("string",  "foo")]
    [InlineData("string",  "1")]
    [InlineData("string",  "")]
    [InlineData("string",  "1.5")]
    public void ValidTypes_Converting_ConvertsSuccessfully(string type, string value)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeTrue();
        error.Should().BeNull();
        instance.Should().NotBeNull();
        instance.ToJsonString().Trim('"').Should().Be(value);
    }
}