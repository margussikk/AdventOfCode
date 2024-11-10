namespace AdventOfCode.Year2022.Day20;

internal class Number
{
    public long Value { get; init; }

    public static Number Parse(string value)
    {
        return new Number
        {
            Value = long.Parse(value)
        };
    }
}
