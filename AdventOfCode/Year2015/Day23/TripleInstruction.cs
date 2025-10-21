namespace AdventOfCode.Year2015.Day23;

internal class TripleInstruction : Instruction
{
    public int Register { get; }

    public TripleInstruction(int register)
    {
        Register = register;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        registers[Register] *= 3;
        instructionPointer++;
    }
}
