namespace AdventOfCode.Year2021.Day24;

internal class VariableParameter(int variable) : IInstructionParameter
{
    public int Variable { get; } = variable;
}
