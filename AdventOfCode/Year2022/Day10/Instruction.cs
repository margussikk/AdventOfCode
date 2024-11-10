namespace AdventOfCode.Year2022.Day10;

internal abstract class Instruction
{
    public int CyclesCount { get; protected init; }

    public static Instruction Parse(string line)
    {
        var splits = line.Split(' ');
        return splits[0] switch
        {
            "noop" => new NoOpInstruction(),
            "addx" => new AddXInstruction(int.Parse(splits[1])),
            _ => throw new InvalidOperationException()
        };
    }
}
