using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.UnitTests;

public class PrimitiveJsonConverterTests
{
    [Theory]
    [InlineData(InstanceType.Integer, "one")]
    [InlineData(InstanceType.Number, "one")]
    [InlineData(InstanceType.Boolean, "one")]
    public void InvalidTypes_Converting_ReportError(InstanceType type, string value)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeFalse();
        instance.Should().BeNull();
        error.Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [InlineData(InstanceType.Integer, "1")]
    [InlineData(InstanceType.Integer, "1.0")]
    [InlineData(InstanceType.Integer, "-0")]
    [InlineData(InstanceType.Integer,  "-9007199254740991")]
    [InlineData(InstanceType.Integer,  "9007199254740992")]
    [InlineData(InstanceType.Number, "1")]
    [InlineData(InstanceType.Number, "1.0")]
    [InlineData(InstanceType.Number, "-0")]
    [InlineData(InstanceType.Number,  "-9007199254740991")]
    [InlineData(InstanceType.Number,  "9007199254740992")]
    [InlineData(InstanceType.Number,  "1.5")]
    [InlineData(InstanceType.Boolean,  "true")]
    [InlineData(InstanceType.Boolean,  "false")]
    [InlineData(InstanceType.String,  "false")]
    [InlineData(InstanceType.String,  "foo")]
    [InlineData(InstanceType.String,  "1")]
    [InlineData(InstanceType.String,  "")]
    [InlineData(InstanceType.String,  "1.5")]
    public void ValidTypes_Converting_ConvertsSuccessfully(InstanceType type, string value)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeTrue();
        error.Should().BeNull();
        instance.Should().NotBeNull();
        instance.ToJsonString().Trim('"').Should().Be(value);
    }
}