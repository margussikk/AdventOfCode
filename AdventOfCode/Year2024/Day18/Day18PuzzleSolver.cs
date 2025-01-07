using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Numerics;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2024.Day18;

[Puzzle(2024, 18, "RAM Run")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<GridCoordinate> _byteCoordinates = [];
    private readonly GridCoordinate _startCoordinate = new(0, 0);
    private readonly GridCoordinate _endCoordinate = new(70, 70);

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

        var pathFinder = new GridPathFinder<bool>(grid)
            .SetCellFilter(GridPathFinder<bool>.DefaultBitGridCellFilter);
        
        var answer = pathFinder.FindShortestPathLength(_startCoordinate, _endCoordinate);

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

            var pathFinder = new GridPathFinder<bool>(grid)
                .SetCellFilter(GridPathFinder<bool>.DefaultBitGridCellFilter);

            var steps = pathFinder.FindShortestPathLength(_startCoordinate, _endCoordinate);
            if (steps == int.MaxValue)
            {
                numberRange = numberRange.SplitAfter(pivot)[0];
            }
            else
            {
                numberRange = numberRange.SplitAfter(pivot)[1];
            }
        }

        var answer = _byteCoordinates[numberRange.End];

        return new PuzzleAnswer($"{answer.Column},{answer.Row}", "65,6");
    }
}