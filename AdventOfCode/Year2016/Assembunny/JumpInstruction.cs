namespace AdventOfCode.Year2016.Assembunny;

internal class JumpInstruction : Instruction
{
    public InstructionArgument Argument1 { get; }

    public InstructionArgument Argument2 { get; }

    public JumpInstruction(InstructionArgument argument1, InstructionArgument argument2)
    {
        Argument1 = argument1;
        Argument2 = argument2;
    }

    public override void Execute(Instruction[] instructions, int[] registers, ref int instructionPointer)
    {
        var value = Argument1.Value ?? registers[Argument1.Register!.Value];
        var offset = Argument2.Value ?? registers[Argument2.Register!.Value];

        if (value != 0)
        {
            instructionPointer += offset;
        }
        else
        {
            instructionPointer++;
        }
    }
}
