using AdventOfCode.Framework.Puzzle;
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
        var processedCoordinates = new HashSet<Coordinate2D>();
        var areas = new int[_coordinates.Count];

        var minCoordinate = new Coordinate2D(_coordinates.Select(c => c.X).Min(), _coordinates.Select(c => c.Y).Min());
        var maxCoordinate = new Coordinate2D(_coordinates.Select(c => c.X).Max(), _coordinates.Select(c => c.Y).Max());

        var region = new Region2D(minCoordinate, maxCoordinate);

        var found = true;
        for (var manhattanDistance = 1; found && manhattanDistance < int.MaxValue; manhattanDistance++)
        {
            found = false;
            var measureCoordinates = _coordinates
                .SelectMany(c => c.ManhattanCoordinates(manhattanDistance))
                .Where(c => region.InBounds(c) && !processedCoordinates.Contains(c));

            foreach (var measureCoordinate in measureCoordinates)
            {
                var distances = _coordinates
                    .Select(c => c.ManhattanDistanceTo(measureCoordinate))
                    .ToList();

                var minDistance = distances.Min();
                var shortestDistanceAreaIndexes = distances
                    .Select((distance, index) => (Distance: distance, Index: index))
                    .Where(x => x.Distance == minDistance)
                    .Select(x => x.Index)
                    .ToList();

                if (shortestDistanceAreaIndexes.Count == 1)
                {
                    if (measureCoordinate.X == minCoordinate.X || measureCoordinate.X == maxCoordinate.X ||
                        measureCoordinate.Y == minCoordinate.Y || measureCoordinate.Y == maxCoordinate.Y)
                    {
                        // Infinite
                        areas[shortestDistanceAreaIndexes[0]] = int.MaxValue;
                    }
                    else if (areas[shortestDistanceAreaIndexes[0]] != int.MaxValue)
                    {
                        areas[shortestDistanceAreaIndexes[0]]++;
                        found = true;
                    }
                }

                processedCoordinates.Add(measureCoordinate);
            }
        }

        var answer = areas.Where(a => a != int.MaxValue).Max();

        return new PuzzleAnswer(answer, 3401);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var processedCoordinates = new HashSet<Coordinate2D>();
        var regionSize = 0;

        var found = true;
        for (var manhattanDistance = 1; found && manhattanDistance < int.MaxValue; manhattanDistance++)
        {
            found = false;
            var measureCoordinates = _coordinates
                .SelectMany(c => c.ManhattanCoordinates(manhattanDistance))
                .Where(c => !processedCoordinates.Contains(c));

            foreach (var measureCoordinate in measureCoordinates)
            {
                var totalManhattanDistance = _coordinates.Sum(c => c.ManhattanDistanceTo(measureCoordinate));
                if (totalManhattanDistance < 10_000)
                {
                    regionSize++;
                    found = true;
                }

                processedCoordinates.Add(measureCoordinate);
            }
        }


        return new PuzzleAnswer(regionSize, 49327);
    }
}