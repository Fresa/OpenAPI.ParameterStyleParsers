using JetBrains.Annotations;

namespace OpenAPI.ParameterStyleParsers;

/// <summary>
/// An OpenAPI parameter
/// </summary>
[PublicAPI]
public interface IParameter
{
    /// <summary>
    /// The name of the parameter
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Is the parameter the body directive?
    /// </summary>
    bool InBody { get; }

    /// <summary>
    /// Is the parameter located in the header?
    /// </summary>
    bool InHeader { get; }

    /// <summary>
    /// Is the parameter located in the path?
    /// </summary>
    bool InPath { get; }

    /// <summary>
    /// Is the parameter located in the query?
    /// </summary>
    bool InQuery { get; }

    /// <summary>
    /// Is the parameter located in the form data?
    /// </summary>
    bool InFormData { get; }

    /// <summary>
    /// Is the parameter located in the cookie?
    /// </summary>
    bool InCookie { get; }
}