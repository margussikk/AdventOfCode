namespace AdventOfCode.Year2021.Day24;

internal class NumberParameter(int number) : IInstructionParameter
{
    public int Number { get; } = number;
}
