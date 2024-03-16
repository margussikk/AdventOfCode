using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day03;

internal class Instruction
{
    public GridDirection Direction { get; private set; }

    public int Steps { get; private set; }


    public static Instruction Parse(string input)
    {
        var instruction = new Instruction
        {
            Direction = input[0] switch
            {
                'R' => GridDirection.Right,
                'U' => GridDirection.Up,
                'D' => GridDirection.Down,
                'L' => GridDirection.Left,
                _ => throw new InvalidOperationException("Failed to parse instruction direction")
            },
            Steps = int.Parse(input[1..])
        };

        return instruction;
    }
}
