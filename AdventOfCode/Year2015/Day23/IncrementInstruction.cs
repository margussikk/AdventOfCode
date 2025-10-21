namespace AdventOfCode.Year2015.Day23;

internal class IncrementInstruction : Instruction
{
    public int Register { get; }

    public IncrementInstruction(int register)
    {
        Register = register;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        registers[Register]++;
        instructionPointer++;
    }
}
