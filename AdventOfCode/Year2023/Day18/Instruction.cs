using AdventOfCode.Utilities.Geometry;
using System.Globalization;

namespace AdventOfCode.Year2023.Day18;

internal class Instruction
{
    public GridDirection Direction { get; private set; }

    public int Distance { get; private set; }

    public static Instruction Part1Parse(string input)
    {
        var splits = input.Split(' ');

        var instruction = new Instruction()
        {
            Direction = splits[0][0] switch
            {
                'R' => GridDirection.Right,
                'D' => GridDirection.Down,
                'L' => GridDirection.Left,
                'U' => GridDirection.Up,
                _ => throw new InvalidOperationException(),
            },
            Distance = int.Parse(splits[1])
        };

        return instruction;
    }

    public static Instruction Part2Parse(string input)
    {
        var splits = input.Split(' ');

        var value = splits[2][2..^1];

        var instruction = new Instruction()
        {
            Direction = value[5] switch
            {
                '0' => GridDirection.Right,
                '1' => GridDirection.Down,
                '2' => GridDirection.Left,
                '3' => GridDirection.Up,
                _ => throw new InvalidOperationException(),
            },
            Distance = int.Parse(value[..5], NumberStyles.HexNumber)
        };

        return instruction;
    }
}
