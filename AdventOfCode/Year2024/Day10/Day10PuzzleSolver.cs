using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2024.Day10;

[Puzzle(2024, 10, "Hoof It")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private Grid<int> _grid = new(0, 0);
    private GridPathFinder<int> _gridPathFinder = new(new(0, 0));

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(x => x.ParseToDigit());
        
        _gridPathFinder = new GridPathFinder<int>(_grid)
            .UseFilterFunction((worker, cell) => cell.Object == worker.Cost + 1);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _grid.Where(cell => cell.Object == 0)
                          .Select(cell => _gridPathFinder.FindAllPathCosts(cell.Coordinate, EndCondition).Count)
                          .Sum();

        return new PuzzleAnswer(answer, 489);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _grid.Where(cell => cell.Object == 0)
                          .SelectMany(cell => _gridPathFinder.FindAllPathCosts(cell.Coordinate, EndCondition).Values.Select(x => x.Count))
                          .Sum();

        return new PuzzleAnswer(answer, 1086);
    }

    private static bool EndCondition(GridCoordinatePathWalker walker) => walker.Cost == 9;
}