using System.Text.Json;
using OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenAPI_20;

public class PrimitiveJsonConverterTests
{
    [Theory]
    [InlineData("integer", "one")]
    [InlineData("integer", "false")]
    [InlineData("integer", "1.5")]
    [InlineData("integer",  "-9007199254740992")]
    [InlineData("integer",  "9007199254740993")]
    [InlineData("number", "one")]
    [InlineData("number", "false")]
    [InlineData("boolean", "test")]
    [InlineData("boolean", "1")]
    public void InvalidTypes_Converting_ReportError(string type, string value)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeFalse();
        instance.Should().BeNull();
        error.Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [InlineData("integer", "1", JsonValueKind.Number)]
    [InlineData("integer", "1.0", JsonValueKind.Number)]
    [InlineData("integer", "-0", JsonValueKind.Number)]
    [InlineData("integer",  "-9007199254740991", JsonValueKind.Number)]
    [InlineData("integer",  "9007199254740992", JsonValueKind.Number)]
    [InlineData("number", "1", JsonValueKind.Number)]
    [InlineData("number", "1.0", JsonValueKind.Number)]
    [InlineData("number", "-0", JsonValueKind.Number)]
    [InlineData("number",  "-9007199254740991", JsonValueKind.Number)]
    [InlineData("number",  "9007199254740992", JsonValueKind.Number)]
    [InlineData("number",  "1.5", JsonValueKind.Number)]
    [InlineData("boolean",  "true", JsonValueKind.True)]
    [InlineData("boolean",  "false", JsonValueKind.False)]
    [InlineData("string",  "false", JsonValueKind.String)]
    [InlineData("string",  "foo", JsonValueKind.String)]
    [InlineData("string",  "1", JsonValueKind.String)]
    [InlineData("string",  "", JsonValueKind.String)]
    [InlineData("string",  "1.5", JsonValueKind.String)]
    public void ValidTypes_Converting_ConvertsSuccessfully(string type, string value, JsonValueKind jsonValueKind)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeTrue();
        error.Should().BeNull();
        instance.Should().NotBeNull();
        instance.ToJsonString().Trim('"').Should().Be(value);
        instance.GetValueKind().Should().Be(jsonValueKind);
    }
}