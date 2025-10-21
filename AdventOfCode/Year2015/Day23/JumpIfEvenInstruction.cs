namespace AdventOfCode.Year2015.Day23;

internal class JumpIfEvenInstruction : Instruction
{
    public int Register { get; }
    public int Offset { get; }

    public JumpIfEvenInstruction(int register, int offset)
    {
        Register = register;
        Offset = offset;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        if (registers[Register] % 2 == 0)
        {
            instructionPointer += Offset;
        }
        else
        {
            instructionPointer++;
        }
    }
}
