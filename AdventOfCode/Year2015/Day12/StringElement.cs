namespace AdventOfCode.Year2015.Day12;

internal class StringElement : Element
{
    public string Value { get; private set; } = string.Empty;

    public override int SumOfNumbers(bool notRed)
    {
        return 0;
    }

    public static StringElement Parse(ref ReadOnlySpan<char> span)
    {
        ExpectAndSkip(ref span, '"');

        var endIndex = span.IndexOf('"');
        var value = new string(span[..endIndex]);
        span = span[endIndex..];

        ExpectAndSkip(ref span, '"');

        return new StringElement
        {
            Value = value,
        };
    }
}
