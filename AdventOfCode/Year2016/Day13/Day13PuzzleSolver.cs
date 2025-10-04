using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Numerics;
using System.Numerics;

namespace AdventOfCode.Year2016.Day13;

[Puzzle(2016, 13, "A Maze of Twisty Little Cubicles")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private int _favoriteNumber;
    public void ParseInput(string[] inputLines)
    {
        _favoriteNumber = int.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        var startCoordinate = new GridCoordinate(1, 1);
        var endCoordinate = new GridCoordinate(31, 39);

        var gridWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);
        var visited = new HashSet<GridCoordinate> { gridWalker.Coordinate };

        var queue = new Queue<GridWalker>();
        queue.Enqueue(gridWalker);
        while (queue.TryDequeue(out gridWalker))
        {
            if (gridWalker.Coordinate == endCoordinate)
            {
                answer = gridWalker.Steps;
                break;
            }

            var nextCoordinates = gridWalker.Coordinate
                .SideNeighbors()
                .Where(coordinate => coordinate.Row >= 0 && coordinate.Column >= 0 && IsOpenSpace(coordinate));

            foreach (var nextCoordinate in nextCoordinates)
            {
                if (visited.Add(nextCoordinate))
                {
                    var newWalker = new GridWalker(gridWalker.StartCoordinate, nextCoordinate, GridDirection.None, gridWalker.Steps + 1);
                    queue.Enqueue(newWalker);
                }
            }
        }

        return new PuzzleAnswer(answer, 86);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var startCoordinate = new GridCoordinate(1, 1);

        var gridWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);
        var visited = new HashSet<GridCoordinate> { gridWalker.Coordinate };

        var queue = new Queue<GridWalker>();
        queue.Enqueue(gridWalker);
        while (queue.TryDequeue(out gridWalker))
        {
            if (gridWalker.Steps >= 50)
            {
                continue;
            }

            var nextCoordinates = gridWalker.Coordinate
                .SideNeighbors()
                .Where(coordinate => coordinate.Row >= 0 && coordinate.Column >= 0 && IsOpenSpace(coordinate));

            foreach (var nextCoordinate in nextCoordinates)
            {
                if (visited.Add(nextCoordinate))
                {
                    var newWalker = new GridWalker(gridWalker.StartCoordinate, nextCoordinate, GridDirection.None, gridWalker.Steps + 1);
                    queue.Enqueue(newWalker);
                }
            }
        }

        var answer = visited.Count;

        return new PuzzleAnswer(answer, 127);
    }

    private bool IsOpenSpace(GridCoordinate coordinate)
    {
        var value = coordinate.Row * coordinate.Row + 3 * coordinate.Row + 2 * coordinate.Row * coordinate.Column + coordinate.Column + coordinate.Column * coordinate.Column + _favoriteNumber;
        return BitOperations.PopCount((uint)value) % 2 == 0;
    }
}