namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

internal sealed class TsvArrayValueParser(Parameter parameter) : CharacterSeparatedValuesArrayValueParser(parameter)
{
    protected override char Separator => '\t';
}