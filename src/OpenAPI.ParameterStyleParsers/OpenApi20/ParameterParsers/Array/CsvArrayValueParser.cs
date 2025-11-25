namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

internal sealed class CsvArrayValueParser(Parameter parameter) : CharacterSeparatedValuesArrayValueParser(parameter)
{
    protected override char Separator => ',';
}