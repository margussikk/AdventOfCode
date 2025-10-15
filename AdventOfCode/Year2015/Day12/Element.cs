namespace AdventOfCode.Year2015.Day12;

internal abstract class Element
{
    protected static void ExpectAndSkip(ref ReadOnlySpan<char> span, char expectation)
    {
        if (span[0] != expectation)
        {
            throw new InvalidOperationException($"Was expecting {expectation}");
        }

        span = span[1..];
    }

    public abstract int SumOfNumbers(bool notRed);
}
