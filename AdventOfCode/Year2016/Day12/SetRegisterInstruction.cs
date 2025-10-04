namespace AdventOfCode.Year2016.Day12;

internal class SetRegisterInstruction : Instruction
{
    public int Value { get; }
    public int Register { get; }
    
    public SetRegisterInstruction(int value, int register)
    {
        Value = value;
        Register = register;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        registers[Register] = Value;
        instructionPointer++;
    }
}
