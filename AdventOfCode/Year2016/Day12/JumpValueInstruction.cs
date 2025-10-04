namespace AdventOfCode.Year2016.Day12;

internal class JumpValueInstruction : Instruction
{
    public int Value { get; }
    public int Offset { get; }

    public JumpValueInstruction(int conditionValue, int offset)
    {
        Value = conditionValue;
        Offset = offset;
    }

    public override void Execute(int[] registers, ref int instructionPointer)
    {
        if (Value != 0)
            instructionPointer += Offset;
        else
            instructionPointer++;
    }
}
