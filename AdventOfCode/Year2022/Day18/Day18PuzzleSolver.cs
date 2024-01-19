using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using Microsoft.CodeAnalysis;

namespace AdventOfCode.Year2022.Day18;

[Puzzle(2022, 18, "Boiling Boulders")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<Coordinate3D> _cubeCoordinates = [];

    public void ParseInput(string[] inputLines)
    {
        _cubeCoordinates = inputLines.Select(Coordinate3D.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var cubeLocations = new HashSet<Coordinate3D>(_cubeCoordinates);

        var answer = _cubeCoordinates
            .Sum(x => x.Sides()
                       .Count(c => !cubeLocations.Contains(c)));

        return new PuzzleAnswer(answer, 4580);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cubeLocations = new HashSet<Coordinate3D>();
        var minLocation = new Coordinate3D(int.MaxValue, int.MaxValue, int.MaxValue);
        var maxLocation = new Coordinate3D(int.MinValue, int.MinValue, int.MinValue);

        foreach (var cubeLocation in _cubeCoordinates)
        {
            cubeLocations.Add(cubeLocation);

            minLocation = new Coordinate3D(
                Math.Min(minLocation.X, cubeLocation.X),
                Math.Min(minLocation.Y, cubeLocation.Y),
                Math.Min(minLocation.Z, cubeLocation.Z));

            maxLocation = new Coordinate3D(
                Math.Max(maxLocation.X, cubeLocation.X),
                Math.Max(maxLocation.Y, cubeLocation.Y),
                Math.Max(maxLocation.Z, cubeLocation.Z));
        }

        var scanMinLocation = new Coordinate3D(minLocation.X - 1, minLocation.Y - 1, minLocation.Z - 1);
        var scanMaxLocation = new Coordinate3D(maxLocation.X + 1, maxLocation.Y + 1, maxLocation.Z + 1);

        // Scan
        var answer = 0;

        // Find exterior cubes using BFS
        var visited = new HashSet<Coordinate3D>();
        var queue = new Queue<Coordinate3D>();

        queue.Enqueue(scanMinLocation);
        while (queue.TryDequeue(out var currentCoordinate))
        {
            if (visited.Contains(currentCoordinate))
            {
                continue;
            }

            visited.Add(currentCoordinate);

            // Count connected to cubes
            answer += currentCoordinate.Sides()
                                       .Count(cubeLocations.Contains);

            foreach (var location in GetSideLocations(currentCoordinate))
            {
                if (!cubeLocations.Contains(location))
                {
                    queue.Enqueue(location);
                }
            }
        }

        return new PuzzleAnswer(answer, 2610);

        IEnumerable<Coordinate3D> GetSideLocations(Coordinate3D currentCoordinate)
        {
            foreach (var coordinate in currentCoordinate.Sides())
            {
                if (coordinate.X < scanMinLocation.X || coordinate.X > scanMaxLocation.X ||
                    coordinate.Y < scanMinLocation.Y || coordinate.Y > scanMaxLocation.Y ||
                    coordinate.Z < scanMinLocation.Z || coordinate.Z > scanMaxLocation.Z)
                {
                    continue;
                }

                yield return coordinate;
            }
        }
    }
}