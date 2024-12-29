using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day23;

[Puzzle(2018, 23, "Experimental Emergency Teleportation")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private List<Nanobot> _nanobots = [];

    public void ParseInput(string[] inputLines)
    {
        _nanobots = inputLines.Select(Nanobot.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var nanobot = _nanobots.MaxBy(x => x.SignalRadius)!;

        var answer = _nanobots.Count(nb => nb.Coordinate.ManhattanDistanceTo(nanobot.Coordinate) <= nanobot.SignalRadius);

        return new PuzzleAnswer(answer, 889);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {        
        var coordinates = new HashSet<Coordinate3D>();

        foreach (var nanobot in _nanobots)
        {
            coordinates.Add(new Coordinate3D(nanobot.Coordinate.X + nanobot.SignalRadius, nanobot.Coordinate.Y, nanobot.Coordinate.Z));
            coordinates.Add(new Coordinate3D(nanobot.Coordinate.X - nanobot.SignalRadius, nanobot.Coordinate.Y, nanobot.Coordinate.Z));

            coordinates.Add(new Coordinate3D(nanobot.Coordinate.X, nanobot.Coordinate.Y + nanobot.SignalRadius, nanobot.Coordinate.Z));
            coordinates.Add(new Coordinate3D(nanobot.Coordinate.X, nanobot.Coordinate.Y - nanobot.SignalRadius, nanobot.Coordinate.Z));

            coordinates.Add(new Coordinate3D(nanobot.Coordinate.X, nanobot.Coordinate.Y, nanobot.Coordinate.Z + nanobot.SignalRadius));
            coordinates.Add(new Coordinate3D(nanobot.Coordinate.X, nanobot.Coordinate.Y, nanobot.Coordinate.Z - nanobot.SignalRadius));
        }

        var candidateCoordinate = coordinates.MaxBy(coordinate => _nanobots.Count(nb => nb.Coordinate.ManhattanDistanceTo(coordinate) <= nb.SignalRadius));

        // Use path finding to find the best distance
        var answer = long.MaxValue;
        var highestCount = int.MinValue;

        var visited = new HashSet<Coordinate3D>();

        var queue = new PriorityQueue<Coordinate3D, long>();
        queue.Enqueue(candidateCoordinate, answer);

        while (queue.TryDequeue(out var coordinate, out _))
        {
            if (!visited.Add(coordinate))
            {
                continue;
            }

            var count = _nanobots.Count(nb => nb.Coordinate.ManhattanDistanceTo(coordinate) <= nb.SignalRadius);
            if (count > highestCount)
            {
                highestCount = count;
            }
            else if (count < highestCount)
            {
                continue;
            }

            var distance = coordinate.ManhattanDistanceTo(Coordinate3D.Zero);
            if (distance < answer)
            {
                answer = distance;
            }
            else if (distance > answer)
            {
                continue;
            }

            foreach (var nextCoordinate in coordinate.SideNeighbors())
            {
                var nextDistance = nextCoordinate.ManhattanDistanceTo(Coordinate3D.Zero);
                queue.Enqueue(nextCoordinate, nextDistance);
            }
        }

        return new PuzzleAnswer(answer, 160646364);
    }
}