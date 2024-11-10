namespace AdventOfCode.Year2021.Day24;

internal class VariableParameter : IInstructionParameter
{
    public int Variable { get; }

    public VariableParameter(int variable)
    {
        Variable = variable;
    }
}
