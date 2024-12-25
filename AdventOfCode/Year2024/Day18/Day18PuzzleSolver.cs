using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2024.Day18;

[Puzzle(2024, 18, "RAM Run")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<GridCoordinate> _byteCoordinates = [];
    private GridCoordinate _startCoordinate = new(0, 0);
    private GridCoordinate _endCoordinate = new(70, 70);

    public void ParseInput(string[] inputLines)
    {
        _byteCoordinates = inputLines.Select(GridCoordinate.Parse)
                                     .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = new BitGrid(71, 71);

        foreach (var coordinate in _byteCoordinates.Take(1024))
        {
            grid[coordinate] = true;
        }

        var answer = FindFewestSteps(grid);

        return new PuzzleAnswer(answer, 370);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var numberRange = new NumberRange<int>(0, _byteCoordinates.Count - 1);

        while (numberRange.Length > 1)
        {
            var pivot = (numberRange.Start + numberRange.End) / 2;

            var grid = new BitGrid(71, 71);
            foreach (var coordinate in _byteCoordinates.Take(pivot + 1))
            {
                grid[coordinate] = true;
            }

            var steps = FindFewestSteps(grid);
            if (steps == int.MaxValue)
            {
                numberRange = new NumberRange<int>(numberRange.Start, pivot);
            }
            else
            {
                numberRange = new NumberRange<int>(pivot + 1, numberRange.End);
            }
        }

        var answer = _byteCoordinates[numberRange.End];

        return new PuzzleAnswer($"{answer.Column},{answer.Row}", "65,6");
    }

    private int FindFewestSteps(BitGrid grid)
    {
        var visited = new BitGrid(grid.Height, grid.Width);

        var queue = new PriorityQueue<GridCoordinate, int>();
        queue.Enqueue(_startCoordinate, 0);

        while (queue.TryDequeue(out var coordinate, out var steps))
        {
            if (coordinate == _endCoordinate)
            {
                return steps;
            }

            if (visited[coordinate])
            {
                continue;
            }

            visited[coordinate] = true;

            foreach (var cell in grid.SideNeighbors(coordinate).Where(c => !c.Object))
            {
                queue.Enqueue(cell.Coordinate, steps + 1);
            }
        }

        return int.MaxValue;
    }
}