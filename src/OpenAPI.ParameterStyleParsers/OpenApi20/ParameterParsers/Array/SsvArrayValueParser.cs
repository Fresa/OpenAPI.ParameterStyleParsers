namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

internal sealed class SsvArrayValueParser(Parameter parameter) : CharacterSeparatedValuesArrayValueParser(parameter)
{
    protected override char Separator => ' ';
}