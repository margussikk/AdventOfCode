using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022.Day15;

[Puzzle(2022, 15, "Beacon Exclusion Zone")]
public partial class Day15PuzzleSolver : IPuzzleSolver
{
    private readonly List<Sensor> _sensors = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var matches = InputLineRegex().Matches(line);
            if (matches.Count == 1)
            {
                var match = matches[0];

                var sensorCoordinate = new Coordinate2D(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value));

                var beaconCoordinate = new Coordinate2D(
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value));

                _sensors.Add(new Sensor(sensorCoordinate, beaconCoordinate));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        const int y = 2_000_000;

        var ranges = _sensors
            .Where(s => y >= s.TopCoordinate.Y && y <= s.BottomCoordinate.Y)
            .Select(s => new NumberRange<long>(s.GetAdjustedLeftX(y), s.GetAdjustedRightX(y)))
            .ToArray();

        var rangesLengths = NumberRange<long>.Merge(ranges)
                                             .Sum(r => r.Length);

        // Exclude beacons
        var beacons = _sensors.Where(s => s.BeaconCoordinate.Y == y)
                              .Select(x => x.BeaconCoordinate.X)
                              .Distinct()
                              .Count();

        var answer = rangesLengths - beacons;

        return new PuzzleAnswer(answer, 4725496);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Empty position can exist only in this configuration:
        //
        //               /  \/            \
        //              /   /\             \
        //             /   /  \             \  
        //            /   /    \             \
        //           /    \     \  /\        /
        //           \     \    / /  \      /
        //            \  /\ \  / /    \    /
        //             \/  \ \/ /      \  /
        //             /\   \/\/        \/
        //            /  \  /\/\        /\
        //           /    \/ /\ \      /  \
        //          /       /  \ \    /    \
        //          \      /   /  \  /      \
        //           \     \  /    \/       /
        //            \     \/             /
        //             \    /\            /
        //              \  /  \          /

        // Little diagram for understanding negative and positive slopes
        //          top
        //          /\
        //         /  \
        //        /    \
        //  left /      \ right
        //       \      /
        //        \    /
        //         \  /
        //          \/
        //        bottom

        var coordinateValueRange = new NumberRange<long>(0, 4_000_000L);

        var negativeSlopeLines = new List<Line2D>();
        var positiveSlopeLines = new List<Line2D>();

        foreach (var sensor in _sensors)
        {
            var topRightLine = new Line2D(sensor.TopCoordinate - Vector2D.UnitY, sensor.RightCoordinate + Vector2D.UnitX);
            negativeSlopeLines.Add(topRightLine);

            var leftBottomLine = new Line2D(sensor.LeftCoordinate - Vector2D.UnitX, sensor.BottomCoordinate + Vector2D.UnitY);
            negativeSlopeLines.Add(leftBottomLine);

            var leftTopLine = new Line2D(sensor.LeftCoordinate - Vector2D.UnitX, sensor.TopCoordinate - Vector2D.UnitY);
            positiveSlopeLines.Add(leftTopLine);

            var bottomRightLine = new Line2D(sensor.BottomCoordinate + Vector2D.UnitY, sensor.RightCoordinate + Vector2D.UnitX);
            positiveSlopeLines.Add(bottomRightLine);
        }

        foreach (var negativeSlopeLine in negativeSlopeLines)
        {
            foreach (var positiveSlopeLine in positiveSlopeLines)
            {
                if (!negativeSlopeLine.TryFindIntersectionCoordinate(positiveSlopeLine,
                        out var intersectionCoordinate) ||
                    !coordinateValueRange.Contains(intersectionCoordinate.X) ||
                    !coordinateValueRange.Contains(intersectionCoordinate.Y)) continue;
                
                var notDetectable = _sensors.TrueForAll(s => !s.Detectable(intersectionCoordinate));
                if (!notDetectable) continue;
                
                var answer = 4_000_000L * intersectionCoordinate.X + intersectionCoordinate.Y;
                return new PuzzleAnswer(answer, 12051287042458L);
            }
        }

        return new PuzzleAnswer("ERROR", "NOT ERROR");
    }

    [GeneratedRegex(@"Sensor at x=(\-*\d+), y=(\-*\d+): closest beacon is at x=(\-*\d+), y=(\-*\d+)")]
    private static partial Regex InputLineRegex();
}