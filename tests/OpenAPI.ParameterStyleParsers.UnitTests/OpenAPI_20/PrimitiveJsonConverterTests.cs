using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_20;

public class PrimitiveJsonConverterTests
{
    [Theory]
    [InlineData("integer", "1", "1")]
    [InlineData("integer", "1.0", "1.0")]
    [InlineData("integer", "-0", "-0")]
    [InlineData("integer",  "-9007199254740991", "-9007199254740991")]
    [InlineData("integer",  "9007199254740992", "9007199254740992")]
    // out-of-bound for integer type, still formatted as integer as there is no validation
    [InlineData("integer",  "9007199254740993", "9007199254740993")]
    // convert to string if the value is not an integer, but in fact looks like a string
    [InlineData("integer",  "foo", "\"foo\"")]
    [InlineData("number", "1", "1")]
    [InlineData("number", "1.0", "1.0")]
    [InlineData("number", "-0", "-0")]
    [InlineData("number",  "-9007199254740991", "-9007199254740991")]
    [InlineData("number",  "9007199254740992", "9007199254740992")]
    [InlineData("number",  "1.5", "1.5")]
    // convert to string if the value is not a number, but in fact looks like a string
    [InlineData("number",  "foo", "\"foo\"")]
    [InlineData("boolean",  "true", "true")]
    [InlineData("boolean",  "false", "false")]
    // convert to string if the value is not a boolean, but in fact looks like a string
    [InlineData("boolean",  "foo", "\"foo\"")]
    [InlineData("string",  "false", "\"false\"")]
    [InlineData("string",  "foo", "\"foo\"")]
    [InlineData("string",  "1", "\"1\"")]
    [InlineData("string",  "", "\"\"")]
    [InlineData("string",  "1.5", "\"1.5\"")]
    public void ValidTypes_Converting_ConvertsSuccessfully(string type, string value, string jsonValue)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeTrue();
        error.Should().BeNull();
        instance.Should().NotBeNull();
        instance.ToJsonString().Should().Be(jsonValue);
    }
}