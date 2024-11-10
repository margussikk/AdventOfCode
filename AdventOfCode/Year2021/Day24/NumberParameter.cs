namespace AdventOfCode.Year2021.Day24;

internal class NumberParameter : IInstructionParameter
{
    public int Number { get; }

    public NumberParameter(int number)
    {
        Number = number;
    }
}
