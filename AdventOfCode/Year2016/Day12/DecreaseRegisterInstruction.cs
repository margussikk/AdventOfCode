namespace AdventOfCode.Year2016.Day12;

internal class DecreaseRegisterInstruction : Instruction
{
    public int Register { get; }

    public DecreaseRegisterInstruction(int register)
    {
        Register = register;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        registers[Register]--;
        instructionPointer++;
    }
}
