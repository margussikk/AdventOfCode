namespace AdventOfCode.Year2016.Assembunny;

internal class OutInstruction : Instruction
{
    public InstructionArgument Argument { get; }

    public OutInstruction(InstructionArgument argument)
    {
        Argument = argument;
    }

    public override void Execute(Instruction[] instructions, int[] registers, ref int instructionPointer)
    {
        var value = Argument.Value ?? registers[Argument.Register!.Value];

        Console.WriteLine(value);

        instructionPointer++;
    }
}
