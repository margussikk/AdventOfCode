using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2025.Day04;

[Puzzle(2025, 4, "Printing Department")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private Grid<bool> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(c => c == '@');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _grid.Count(c => c.Object &&
                                      _grid.AroundNeighbors(c.Coordinate)
                                           .Count(n => n.Object) < 4);

        return new PuzzleAnswer(answer, 1419);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        var grid = _grid.Clone();

        var queue = new Queue<GridCoordinate>(grid.Select(c => c.Coordinate));
        while (queue.TryDequeue(out var coordinate))
        {
            if (!grid[coordinate])
            {
                continue;
            }

            var aroundCount = grid.AroundNeighbors(coordinate).Count(n => n.Object);
            if (aroundCount >= 4)
            {
                continue;
            }

            grid[coordinate] = false;
            answer++;
            foreach (var neighbor in grid.AroundNeighbors(coordinate))
            {
                queue.Enqueue(neighbor.Coordinate);
            }
        }

        return new PuzzleAnswer(answer, 8739);
    }
}