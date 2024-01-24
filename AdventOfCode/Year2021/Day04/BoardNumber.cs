namespace AdventOfCode.Year2021.Day04;

internal class BoardNumber(int value)
{
    public int Value { get; private set; } = value;
    public bool Marked { get; set; } = false;
}
