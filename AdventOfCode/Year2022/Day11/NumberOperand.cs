namespace AdventOfCode.Year2022.Day11;

internal class NumberOperand(int value) : Operand
{
    public int Value { get; } = value;
}
