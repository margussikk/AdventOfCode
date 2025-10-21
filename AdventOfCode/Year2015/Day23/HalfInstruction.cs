namespace AdventOfCode.Year2015.Day23;

internal class HalfInstruction : Instruction
{
    public int Register { get; }

    public HalfInstruction(int register)
    {
        Register = register;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        registers[Register] /= 2;
        instructionPointer++;
    }
}
