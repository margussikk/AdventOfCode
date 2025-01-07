using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2022.Day09;

internal class Instruction
{
    public GridDirection Direction { get; private init; }

    public int Steps { get; private init; }

    public static Instruction Parse(string line)
    {
        var splits = line.Split(' ');

        var direction = splits[0] switch
        {
            "L" => GridDirection.Left,
            "R" => GridDirection.Right,
            "U" => GridDirection.Up,
            "D" => GridDirection.Down,
            _ => throw new InvalidOperationException()
        };

        return new Instruction
        {
            Direction = direction,
            Steps = int.Parse(splits[1])
        };
    }
}
