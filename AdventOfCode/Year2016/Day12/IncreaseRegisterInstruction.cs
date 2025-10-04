namespace AdventOfCode.Year2016.Day12;

internal class IncreaseRegisterInstruction : Instruction
{
    public int Register { get; }

    public IncreaseRegisterInstruction(int register)
    {
        Register = register;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        registers[Register]++;
        instructionPointer++;
    }
}
