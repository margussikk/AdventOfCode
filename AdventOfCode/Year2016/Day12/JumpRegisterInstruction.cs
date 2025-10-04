namespace AdventOfCode.Year2016.Day12;

internal class JumpRegisterInstruction : Instruction
{
    public int Register { get; }
    public int Offset { get; }
    
    public JumpRegisterInstruction(int conditionRegister, int offset)
    {
        Register = conditionRegister;
        Offset = offset;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        if (registers[Register] != 0)
            instructionPointer += Offset;
        else
            instructionPointer++;
    }
}
