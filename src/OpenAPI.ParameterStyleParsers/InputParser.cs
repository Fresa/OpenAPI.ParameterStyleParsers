using System.Diagnostics.CodeAnalysis;

namespace OpenAPI.ParameterStyleParsers;

internal sealed class InputParser(string value)
{
    private int _index;

    internal bool Expect(string characters, 
        [NotNullWhen(false)] out string? error)
    {
        if (!value[_index..].StartsWith(characters))
        {
            error = $"Expected '{value}' to have value '{characters}' at position {_index}";
            return false;
        }

        _index += characters.Length;
        error = null;
        return true;
    }

    internal string Current => value[_index..];
}