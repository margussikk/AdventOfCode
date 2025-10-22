namespace AdventOfCode.Utilities.Numerics;

internal static class NumberGenerator
{
    public static IEnumerable<int> From(int start)
    {
        while (start < int.MaxValue) yield return start++;
    }
}
