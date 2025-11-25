namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

internal sealed class PipesArrayValueParser(Parameter parameter) : CharacterSeparatedValuesArrayValueParser(parameter)
{
    protected override char Separator => '|';
}