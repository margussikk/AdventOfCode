using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day15;

internal partial class Disc
{
    public int Id { get; init; }
    public int Positions { get; init; }
    public int StartPosition { get; init; }

    public int GetPositionAtTime(int time)
    {
        return (StartPosition + time) % Positions;
    }

    public override string ToString()
    {
        return $"Disc {Id} has {Positions} positions; at time=0, it is at position {StartPosition}.";
    }

    public static Disc Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var disc = new Disc
        {
            Id = int.Parse(match.Groups[1].Value),
            Positions = int.Parse(match.Groups[2].Value),
            StartPosition = int.Parse(match.Groups[3].Value)
        };

        return disc;
    }

    [GeneratedRegex(@"Disc #(\d+) has (\d+) positions; at time=0, it is at position (\d+).")]
    private static partial Regex InputLineRegex();
}
