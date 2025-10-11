namespace AdventOfCode.Year2016.Assembunny;

internal class IncreaseInstruction : Instruction
{
    public InstructionArgument Argument { get; }

    public IncreaseInstruction(InstructionArgument argument)
    {
        Argument = argument;
    }

    public override void Execute(Instruction[] instructions, int[] registers, ref int instructionPointer)
    {
        if (Argument.Register.HasValue)
        {
            registers[Argument.Register.Value]++;
        }

        instructionPointer++;
    }
}
