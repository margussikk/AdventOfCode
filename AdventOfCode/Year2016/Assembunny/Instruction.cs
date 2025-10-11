using AdventOfCode.Year2016.Day12;

namespace AdventOfCode.Year2016.Assembunny;

internal abstract class Instruction
{
    public abstract void Execute(Instruction[] instructions, int[] registers, ref int instructionPointer);

    public static Instruction Parse(string line)
    {
        var parts = line.Split(' ');

        return parts[0] switch
        {
            "cpy" => new CopyInstruction(InstructionArgument.Parse(parts[1]), InstructionArgument.Parse(parts[2])),
            "inc" => new IncreaseInstruction(InstructionArgument.Parse(parts[1])),
            "dec" => new DecreaseInstruction(InstructionArgument.Parse(parts[1])),
            "jnz" => new JumpInstruction(InstructionArgument.Parse(parts[1]), InstructionArgument.Parse(parts[2])),
            "tgl" => new ToggleInstruction(InstructionArgument.Parse(parts[1])),
            _ => throw new ArgumentException($"Unknown instruction '{parts[0]}'."),
        };
    }
}
