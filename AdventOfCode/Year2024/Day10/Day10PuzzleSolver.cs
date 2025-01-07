using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2024.Day10;

[Puzzle(2024, 10, "Hoof It")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private Grid<int> _grid = new(0, 0);
    private GridPathFinder<int> _gridPathFinder = new(new Grid<int>(0, 0));

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(x => x.ParseToDigit());
        
        _gridPathFinder = new GridPathFinder<int>(_grid)
            .SetCellFilter((worker, cell) => cell.Object == worker.Cost + 1);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(true);

        return new PuzzleAnswer(answer, 489);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(false);

        return new PuzzleAnswer(answer, 1086);
    }

    public int GetAnswer(bool unique)
    {
        var answer = 0;

        foreach (var cell in _grid.Where(cell => cell.Object == 0))
        {
            _gridPathFinder.WalkAllPaths(unique, cell.Coordinate, CountTrails);
        }

        return answer;

        bool CountTrails(GridPathWalker walker)
        {
            if (walker.Cost == 9)
            {
                answer++;
            }

            return true;
        }
    }
}