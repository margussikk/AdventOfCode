namespace AdventOfCode.Year2015.Day23;

internal class JumpIfOneInstruction : Instruction
{
    public int Register { get; }
    public int Offset { get; }

    public JumpIfOneInstruction(int register, int offset)
    {
        Register = register;
        Offset = offset;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        if (registers[Register] == 1)
        {
            instructionPointer += Offset;
        }
        else
        {
            instructionPointer++;
        }
    }
}
