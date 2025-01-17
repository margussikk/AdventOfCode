using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2021.Day09;

[Puzzle(2021, 9, "Smoke Basin")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private Grid<byte> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(character => Convert.ToByte(character - '0'));
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var locations = FindLowPointLocations();

        var answer = locations.Aggregate(0, (acc, current) => acc + _grid[current] + 1);

        return new PuzzleAnswer(answer, 444);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var locations = FindLowPointLocations();

        var answer = locations.Select(GetBasinSize)
                              .OrderByDescending(x => x)
                              .Take(3)
                              .Aggregate((acc, next) => acc * next);

        return new PuzzleAnswer(answer, 1168440);
    }

    private List<GridCoordinate> FindLowPointLocations()
    {
        return _grid.Where(gridCell => gridCell.Object < 9 &&
                                       _grid.SideNeighbors(gridCell.Coordinate)
                                            .All(neighborGridCell => neighborGridCell.Object > gridCell.Object))
                    .Select(gridCell => gridCell.Coordinate)
                    .ToList();
    }

    private int GetBasinSize(GridCoordinate coordinate)
    {
        var pathFinder = new GridPathFinder<byte>(_grid);

        return pathFinder.FloodFill(coordinate, cell => cell.Object < 9).Count;
    }
}