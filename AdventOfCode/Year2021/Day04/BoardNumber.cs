namespace AdventOfCode.Year2021.Day04;

internal class BoardNumber
{
    public int Value { get; }
    public bool Marked { get; set; }

    public BoardNumber(int value)
    {
        Value = value;
    }
}
