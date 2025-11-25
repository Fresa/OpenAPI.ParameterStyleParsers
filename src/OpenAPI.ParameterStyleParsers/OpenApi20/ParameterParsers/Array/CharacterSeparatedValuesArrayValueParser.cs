using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.OpenApi20.ParameterParsers.Array;

internal abstract class CharacterSeparatedValuesArrayValueParser(Parameter parameter) : ArrayValueParser(parameter)
{
    protected abstract char Separator { get; }
    
    public override bool TryParse(
        string? input,
        out JsonNode? array,
        [NotNullWhen(false)] out string? error)
    {
        var value = input;
        if (ValueIncludesKey)
        {
            var valueAndKey = input?.Split('=');
            value = valueAndKey?.Length == 1 ? string.Empty : valueAndKey?.Last();            
        }
        
        var values = value?.Split(Separator);

        return TryGetArrayItems(values, out array, out error);
    }

    protected override string Serialize(string?[] values)
    {
        var serialized = string.Join(Separator,
            values.Select(value => value));
        return ValueIncludesKey ? $"{ParameterName}={serialized}" : serialized;
    }
}