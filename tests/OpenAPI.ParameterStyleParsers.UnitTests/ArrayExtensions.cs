namespace OpenAPI.ParameterStyleParsers.UnitTests;

public static class ArrayExtensions
{
    public static string[] GenerateAllPermutations(this string[] items, char delimiter) => 
        GenerateAllPermutations(items, delimiter.ToString());

    public static string[] GenerateAllPermutations(this string[] items, string delimiter)
    {
        List<string> newItems = [];
        HeapPermutation(items, items.Length, items.Length);
        return newItems.ToArray();

        void HeapPermutation(string[] a, int size, int n)
        {
            // if size becomes 1 then add the obtained permutation
            if (size == 1)
                newItems.Add(string.Join(delimiter, a));

            for (var i = 0; i < size; i++)
            {
                HeapPermutation(a, size - 1, n);

                // if size is odd, swap 0th i.e (first) and
                // (size-1)th i.e (last) element
                if (size % 2 == 1)
                {
                    (a[0], a[size - 1]) = (a[size - 1], a[0]);
                }

                // If size is even, swap ith and
                // (size-1)th i.e (last) element
                else
                {
                    (a[i], a[size - 1]) = (a[size - 1], a[i]);
                }
            }
        }
    }
}