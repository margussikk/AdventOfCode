using System.Text;

namespace AdventOfCode.Year2015.Day12;

internal class NumberElement : Element
{
    public int Value { get; private set; }

    public override int SumOfNumbers(bool notRed)
    {
        return Value;
    }

    public static NumberElement Parse(ref ReadOnlySpan<char> span)
    {
        var stringBuilder = new StringBuilder();

        while (span[0] == '-' || char.IsNumber(span[0]))
        {
            stringBuilder.Append(span[0]);
            span = span[1..];
        }

        return new NumberElement
        {
            Value = int.Parse(stringBuilder.ToString()),
        };
    }
}
