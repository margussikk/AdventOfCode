namespace AdventOfCode.Year2022.Day10;

internal class AddXInstruction : Instruction
{
    public int Parameter { get; }

    public AddXInstruction(int parameter)
    {
        CyclesCount = 2;
        Parameter = parameter;
    }
}
