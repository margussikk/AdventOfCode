namespace AdventOfCode.Year2015.Day23;

internal abstract class Instruction
{
    public abstract void Execute(int[] registers, ref int instructionPointer);

    public static Instruction Parse(string line)
    {
        var parts = line.Split(' ');
        return parts[0] switch
        {
            "hlf" => new HalfInstruction(parts[1][0] - 'a'),
            "tpl" => new TripleInstruction(parts[1][0] - 'a'),
            "inc" => new IncrementInstruction(parts[1][0] - 'a'),
            "jmp" => new JumpInstruction(int.Parse(parts[1].Replace("+", string.Empty))),
            "jie" => new JumpIfEvenInstruction(parts[1][0] - 'a', int.Parse(parts[2].Replace("+", string.Empty))),
            "jio" => new JumpIfOneInstruction(parts[1][0] - 'a', int.Parse(parts[2].Replace("+", string.Empty))),
            _ => throw new InvalidOperationException($"Unknown instruction: {line}"),
        };
    }
}
