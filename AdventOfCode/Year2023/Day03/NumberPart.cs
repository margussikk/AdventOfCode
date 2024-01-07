namespace AdventOfCode.Year2023.Day03;

internal class NumberPart(int row, int column, int length, long number) : EnginePart(row, column, length)
{
    public long Number { get; } = number;
}
