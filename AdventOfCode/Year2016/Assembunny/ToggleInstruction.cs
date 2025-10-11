namespace AdventOfCode.Year2016.Assembunny;

internal class ToggleInstruction : Instruction
{
    public InstructionArgument Argument { get; }

    public ToggleInstruction(InstructionArgument argument)
    {
        Argument = argument;
    }

    public override void Execute(Instruction[] instructions, int[] registers, ref int instructionPointer)
    {
        var offset = Argument.Value ?? registers[Argument.Register!.Value];

        var toggleOffset = instructionPointer + offset;

        if (toggleOffset >= 0 && toggleOffset < instructions.Length)
        {
            instructions[toggleOffset] = instructions[toggleOffset] switch
            {
                IncreaseInstruction increaseInstruction => new DecreaseInstruction(increaseInstruction.Argument),
                DecreaseInstruction decreaseInstruction => new IncreaseInstruction(decreaseInstruction.Argument),
                ToggleInstruction toggleInstruction => new IncreaseInstruction(toggleInstruction.Argument),
                CopyInstruction copyInstruction => new JumpInstruction(copyInstruction.Argument1, copyInstruction.Argument2),
                JumpInstruction jumpInstruction => new CopyInstruction(jumpInstruction.Argument1, jumpInstruction.Argument2),
                _ => throw new InvalidOperationException($"Unexpected instruction")
            };
        }

        instructionPointer++;
    }
}
