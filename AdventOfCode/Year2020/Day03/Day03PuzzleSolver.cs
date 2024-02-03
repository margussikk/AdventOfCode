using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2020.Day03;

[Puzzle(2020, 3, "Toboggan Trajectory")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private BitGrid _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToBitGrid(character => character == '#');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CountTrees(3, 1);

        return new PuzzleAnswer(answer, 181);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = CountTrees(1, 1) *
                     CountTrees(3, 1) *
                     CountTrees(5, 1) *
                     CountTrees(7, 1) *
                     CountTrees(1, 2);

        return new PuzzleAnswer(answer, 1260601650L);
    }


    private long CountTrees(int columnSteps, int rowSteps)
    {
        var trees = 0;
        var row = 0;
        var column = 0;

        while (_grid.InBounds(row, column))
        {
            if (_grid[row, column])
            {
                trees++;
            }

            column = (column + columnSteps) % _grid.Width;
            row += rowSteps;
        }

        return trees;
    }
}