namespace AdventOfCode.Year2016.Assembunny;

internal class CopyInstruction : Instruction
{
    public InstructionArgument Argument1 { get; }

    public InstructionArgument Argument2 { get; }

    public CopyInstruction(InstructionArgument argument1, InstructionArgument argument2)
    {
        Argument1 = argument1;
        Argument2 = argument2;
    }

    public override void Execute(Instruction[] instructions, int[] registers, ref int instructionPointer)
    {
        if (Argument2.Register.HasValue)
        {
            registers[Argument2.Register.Value] = Argument1.Value ?? registers[Argument1.Register!.Value];
        }

        instructionPointer++;
    }
}
