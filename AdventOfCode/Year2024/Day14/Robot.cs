using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Mathematics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Day14;
internal partial class Robot
{
    private const long areaWidth = 101;
    private const long areaHeight = 103;

    public Coordinate2D Position { get; set; }
    public Vector2D Velocity { get; set; }

    public Robot Clone()
    {
        return new Robot
        {
            Position = Position,
            Velocity = Velocity
        };
    }

    public void Move()
    {
        var x = MathFunctions.Modulo(Position.X + Velocity.DX, areaWidth);
        var y = MathFunctions.Modulo(Position.Y + Velocity.DY, areaHeight);

        Position = new Coordinate2D(x, y);
    }

    public static Robot Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        return new Robot
        {
            Position = Coordinate2D.Parse(matches[0].Groups[1].Value),
            Velocity = Vector2D.Parse(matches[0].Groups[2].Value)
        };
    }

    [GeneratedRegex(@"p=(-?\d+,-?\d+) v=(-?\d+,-?\d+)")]
    private static partial Regex InputLineRegex();
}
