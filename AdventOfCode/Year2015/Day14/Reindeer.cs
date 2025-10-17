using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015.Day14;

internal partial class Reindeer
{
    public string Name { get; init; } = string.Empty;
    public int FlySpeed { get; init; }
    public int FlyDuration { get; init; }
    public int RestDuration { get; init; }

    public int CalculateDistanceAfterSeconds(int totalSeconds)
    {
        int cycleDuration = FlyDuration + RestDuration;

        var fullCycles = Math.DivRem(totalSeconds, cycleDuration, out var remainingSeconds);
        int distance = fullCycles * FlySpeed * FlyDuration;
        distance += FlySpeed * Math.Min(remainingSeconds, FlyDuration);
        return distance;
    }

    public static Reindeer Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        return new Reindeer
        {
            Name = match.Groups[1].Value,
            FlySpeed = int.Parse(match.Groups[2].Value),
            FlyDuration = int.Parse(match.Groups[3].Value),
            RestDuration = int.Parse(match.Groups[4].Value)
        };
    }

    [GeneratedRegex(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.")]
    private static partial Regex InputLineRegex();
}
