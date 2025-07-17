namespace OpenAPI.ParameterStyleParsers.UnitTests.Xunit;

internal static class TheoryDataExtensions
{
    public static TheoryData<T1, T2, T3> AddRange<T1, T2, T3>(this TheoryData<T1, T2, T3> theory,
        IEnumerable<(T1, T2, T3)> data)
    {
        foreach (var item in data)
        {
            theory.Add(item.Item1, item.Item2, item.Item3);
        }

        return theory;
    }
    public static TheoryData<T1, T2, T3, T4> AddRange<T1, T2, T3, T4>(this TheoryData<T1, T2, T3, T4> theory,
        IEnumerable<(T1, T2, T3, T4)> data)
    {
        foreach (var item in data)
        {
            theory.Add(item.Item1, item.Item2, item.Item3, item.Item4);
        }

        return theory;
    }
}