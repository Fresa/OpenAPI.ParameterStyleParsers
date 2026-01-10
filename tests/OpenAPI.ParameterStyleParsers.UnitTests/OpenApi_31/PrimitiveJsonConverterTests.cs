using OpenAPI.ParameterStyleParsers.JsonSchema;
using OpenAPI.ParameterStyleParsers.OpenApi31.ParameterParsers.Primitive;

namespace OpenAPI.ParameterStyleParsers.UnitTests.OpenApi_31;

public class PrimitiveJsonConverterTests
{
    [Theory]
    [InlineData(InstanceType.Integer, "1", "1")]
    [InlineData(InstanceType.Integer, "1.0", "1.0")]
    [InlineData(InstanceType.Integer, "-0", "-0")]
    [InlineData(InstanceType.Integer,  "-9007199254740991", "-9007199254740991")]
    [InlineData(InstanceType.Integer,  "9007199254740992", "9007199254740992")]
    // out-of-bound for integer type, still formatted as integer as there is no validation
    [InlineData(InstanceType.Integer,  "9007199254740993", "9007199254740993")]
    // convert to string if the value is not an integer, but in fact looks like a string
    [InlineData(InstanceType.Integer,  "foo", "\"foo\"")]
    [InlineData(InstanceType.Number, "1", "1")]
    [InlineData(InstanceType.Number, "1.0", "1.0")]
    [InlineData(InstanceType.Number, "-0", "-0")]
    [InlineData(InstanceType.Number,  "-9007199254740991", "-9007199254740991")]
    [InlineData(InstanceType.Number,  "9007199254740992", "9007199254740992")]
    [InlineData(InstanceType.Number,  "1.5", "1.5")]
    // convert to string if the value is not a number, but in fact looks like a string
    [InlineData(InstanceType.Number,  "foo", "\"foo\"")]
    [InlineData(InstanceType.Boolean,  "true", "true")]
    [InlineData(InstanceType.Boolean,  "false", "false")]
    // convert to string if the value is not a boolean, but in fact looks like a string
    [InlineData(InstanceType.Boolean,  "foo", "\"foo\"")]
    [InlineData(InstanceType.String,  "false", "\"false\"")]
    [InlineData(InstanceType.String,  "foo", "\"foo\"")]
    [InlineData(InstanceType.String,  "1", "\"1\"")]
    [InlineData(InstanceType.String,  "", "\"\"")]
    [InlineData(InstanceType.String,  "1.5", "\"1.5\"")]
    public void ValidTypes_Converting_ConvertsSuccessfully(InstanceType type, string value, string jsonValue)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeTrue();
        error.Should().BeNull();
        instance.Should().NotBeNull();
        instance.ToJsonString().Should().Be(jsonValue);
    }
    
    [Theory]
    [InlineData(InstanceType.Integer,  null)]
    [InlineData(InstanceType.Number,  null)]
    [InlineData(InstanceType.Boolean,  null)]
    [InlineData(InstanceType.String,  null)]
    [InlineData(InstanceType.Null,  null)]
    public void Null_Converting_ConvertsSuccessfully(InstanceType type, string value)
    {
        PrimitiveJsonConverter.TryConvert(value, type, out var instance, out var error)
            .Should().BeTrue();
        error.Should().BeNull();
        instance.Should().BeNull();
    }
}