using AdventOfCode.Utilities.Geometry;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day10;

internal partial class PointOfLight
{
    public Coordinate2D Position { get; set; }

    public Vector2D Velocity { get; set; }

    public void Move()
    {
        Position = Position + Velocity;
    }

    public static PointOfLight Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var position = new Coordinate2D(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        var velocity = new Vector2D(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

        return new PointOfLight
        {
            Position = position,
            Velocity = velocity
        };
    }

    [GeneratedRegex(@"position=<\s*(-?\d+),\s*(-?\d+)> velocity=<\s*(-?\d+),\s*(-?\d+)>")]
    private static partial Regex InputLineRegex();
}
