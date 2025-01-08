using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2024.Day16;

[Puzzle(2024, 16, "Reindeer Maze")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private GridPathFinder<bool> _pathFinder = new(new Grid<bool>(0, 0));

    private GridPosition _startPosition = new(new(0,0), 0);
    private GridCoordinate _endCoordinate = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        var maze = inputLines.SelectToGrid((character, coordinate) =>
        {
            if (character == 'S')
            {
                _startPosition = new GridPosition(coordinate, GridDirection.Right);
            }
            else if (character == 'E')
            {
                _endCoordinate = coordinate;
            }

            return character == '#';
        });

        _pathFinder = new GridPathFinder<bool>(maze)
            .SetCellFilter(GridPathFinder<bool>.DefaultBitGridCellFilter)
            .SetCostCalculator(CostCalculator);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _pathFinder.FindShortestPathLength(_startPosition, _endCoordinate);

        return new PuzzleAnswer(answer, 102488);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _pathFinder.FindAllShortestPaths(_startPosition, _endCoordinate)
                                .SelectMany(x => x)
                                .Distinct()
                                .Count();

        return new PuzzleAnswer(answer, 559);
    }

    private static int CostCalculator(GridPathWalker walker, GridPosition nextPosition)
    {
        var nextCost = walker.Cost;

        if (nextPosition.Direction != walker.Position.Direction)
        {
            nextCost += 1000;
        }

        if (nextPosition.Coordinate != walker.Position.Coordinate)
        {
            nextCost++;
        }

        return nextCost;
    }
}