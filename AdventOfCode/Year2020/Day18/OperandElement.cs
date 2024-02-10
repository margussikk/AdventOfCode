namespace AdventOfCode.Year2020.Day18;

internal class OperandElement(long value) : IElement
{
    public long Value { get; } = value;

    public override string ToString()
    {
        return $"{Value}";
    }
}
