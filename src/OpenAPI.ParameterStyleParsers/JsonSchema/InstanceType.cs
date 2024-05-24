namespace OpenAPI.ParameterStyleParsers.JsonSchema;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

[Flags]
public enum InstanceType
{
    String = 1,
    Boolean = 2,
    Integer = 4,
    Number = 8,
    Null = 16,
    Array = 32,
    Object = 64
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
