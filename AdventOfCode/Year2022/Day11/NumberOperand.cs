namespace AdventOfCode.Year2022.Day11;

internal class NumberOperand : IOperand
{
    public int Value { get; }

    public NumberOperand(int value)
    {
        Value = value;
    }
}
