using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day06;

[Puzzle(2018, 6, "Chronal Coordinates")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private List<Coordinate2D> _coordinates = [];

    public void ParseInput(string[] inputLines)
    {
        _coordinates = inputLines.Select(Coordinate2D.Parse)
                                 .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var areaSizes = _coordinates.ToDictionary(x => x, x => 0);

        var region = new Region2D(_coordinates);
        foreach (var measureCoordinate in region)
        {
            var distancesLookup = _coordinates.ToLookup(c => c.ManhattanDistanceTo(measureCoordinate))
                                              .MinBy(x => x.Key)!;

            if (distancesLookup.Count() != 1)
            {
                continue;
            }

            var areaCoordinate = distancesLookup.First();

            if (measureCoordinate.X == region.MinCoordinate.X || measureCoordinate.X == region.MaxCoordinate.X ||
                measureCoordinate.Y == region.MinCoordinate.Y || measureCoordinate.Y == region.MaxCoordinate.Y)
            {
                // Infinite
                areaSizes[areaCoordinate] = int.MaxValue;
            }
            else if (areaSizes[areaCoordinate] != int.MaxValue)
            {
                areaSizes.IncrementValue(areaCoordinate, 1);
            }
        }

        var answer = areaSizes.Where(x => x.Value != int.MaxValue)
                              .Max(x => x.Value);

        return new PuzzleAnswer(answer, 3401);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var region = new Region2D(_coordinates);

        var answer = region.Count(rc => _coordinates.Sum(c => c.ManhattanDistanceTo(rc)) < 10_000);

        return new PuzzleAnswer(answer, 49327);
    }
}