using AdventOfCode.Utilities.Geometry;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day23;
internal partial class Nanobot
{
    public Coordinate3D Coordinate { get; private set; }

    public long SignalRadius { get; private set; }

    public static Nanobot Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var coordinate = Coordinate3D.Parse(match.Groups[1].Value);
        var radius = long.Parse(match.Groups[2].Value);

        return new Nanobot
        {
            Coordinate = coordinate,
            SignalRadius = radius,
        };
    }

    [GeneratedRegex(@"pos=<(.+)>, r=(\d+)")]
    private static partial Regex InputLineRegex();
}
