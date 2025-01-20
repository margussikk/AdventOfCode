using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2017.Day21;

[Puzzle(2017, 21, "Fractal Art")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<int, Grid<bool>> _enchantmentOutputs = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var enchantmentRule = EnhancementRule.Parse(line);

            foreach (var pattern in enchantmentRule.Patterns)
            {
                _enchantmentOutputs[pattern] = enchantmentRule.Output;
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = BuildGrid(5).Count(c => c.Object);

        return new PuzzleAnswer(answer, 142);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = BuildGrid(18).Count(c => c.Object);

        return new PuzzleAnswer(answer, 1879071);
    }

    private Grid<bool> BuildGrid(int iterations)
    {
        var grid = ".#./..#/###".Split('/')
                                .SelectToGrid(character => character == '#');

        for (var iteration = 0; iteration < iterations; iteration++)
        {
            if (grid.Width % 2 == 0)
            {
                // Break into 2x2
                grid = Enhance(grid, 2);
            }
            else if (grid.Width % 3 == 0)
            {
                // Break into 3x3
                grid = Enhance(grid, 3);
            }
            else
            {
                // Is it even possible?
                throw new InvalidOperationException($"Grid size is not divisible by 2 nor 3");
            }
        }

        return grid;
    }

    private Grid<bool> Enhance(Grid<bool> grid, int size)
    {
        var newSize = size + 1;
        var newGridSize = (grid.Width / size) * newSize;

        var newGrid = new Grid<bool>(newGridSize, newGridSize);

        for (int currentRow = grid.FirstRow; currentRow <= grid.LastRow; currentRow += size)
        {
            for (int currentColumn = grid.FirstColumn; currentColumn <= grid.LastColumn; currentColumn += size)
            {
                var topLeftCoordinate = new GridCoordinate(currentRow, currentColumn);
                var bottomRightCoordinate = new GridCoordinate(currentRow + size - 1, currentColumn + size - 1);
                var windowGrid = grid.Window(topLeftCoordinate, bottomRightCoordinate);

                var bitmask = GridUtils.GetGridBitmask(windowGrid);
                var output = _enchantmentOutputs[bitmask];

                var newRow = (currentRow / size) * newSize;
                var newColumn = (currentColumn / size) * newSize;
                var coordinate = new GridCoordinate(newRow, newColumn);

                newGrid.CopyFrom(output, coordinate);
            }
        }

        return newGrid;
    }
}