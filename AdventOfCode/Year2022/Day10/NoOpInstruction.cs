namespace AdventOfCode.Year2022.Day10;

internal class NoOpInstruction : Instruction
{
    public NoOpInstruction()
    {
        CyclesCount = 1;
    }
}
