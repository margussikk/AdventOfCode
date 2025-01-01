using AdventOfCode.Utilities.Geometry;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day10;

internal partial class PointOfLight
{
    public Coordinate2D Position { get; private set; }

    public Vector2D Velocity { get; private set; }

    public PointOfLight Clone()
    {
        return new PointOfLight
        {
            Position = Position,
            Velocity = Velocity,
        };
    }

    public void Move()
    {
        Position += Velocity;
    }

    public static PointOfLight Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var position = Coordinate2D.Parse(match.Groups[1].Value.Replace(" ", string.Empty));
        var velocity = Vector2D.Parse(match.Groups[2].Value.Replace(" ", string.Empty));

        return new PointOfLight
        {
            Position = position,
            Velocity = velocity
        };
    }

    [GeneratedRegex(@"position=<(\s*-?\d+,\s*-?\d+)> velocity=<(\s*-?\d+,\s*-?\d+)>")]
    private static partial Regex InputLineRegex();
}
