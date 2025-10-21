namespace AdventOfCode.Year2015.Day23;

internal class JumpInstruction : Instruction
{
    public int Offset { get; }

    public JumpInstruction(int offset)
    {
        Offset = offset;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        instructionPointer += Offset;
    }
}
