using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2021.Day11;

[Puzzle(2021, 11, "Dumbo Octopus")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private Grid<int> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(character => character - '0');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = _grid.Clone();

        var answer = 0;
        for (var step = 0; step < 100; step++)
        {
            answer += grid.Sum(cell => ProcessOctopus(grid, cell.Coordinate));

            ResetEnergyLevels(grid);
        }

        return new PuzzleAnswer(answer, 1620);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        int resetCount;
        var answer = 0;
        var area = _grid.Width * _grid.Height;
        var grid = _grid.Clone();

        do
        {
            foreach (var cell in grid)
            {
                ProcessOctopus(grid, cell.Coordinate);
            }

            resetCount = ResetEnergyLevels(grid);
            answer++;
        }
        while (resetCount != area);

        return new PuzzleAnswer(answer, 371);
    }

    private static int ProcessOctopus(Grid<int> grid, GridCoordinate coordinate)
    {
        grid[coordinate] += 1;
        if (grid[coordinate] == 10) // Flash only for the first time, 9 -> 10
        {
            return 1 + grid.AroundNeighbors(coordinate)
                           .Sum(cell => ProcessOctopus(grid, cell.Coordinate));
        }

        return 0;
    }

    private static int ResetEnergyLevels(Grid<int> grid)
    {
        var count = 0;

        foreach (var gridCell in grid.Where(c => c.Object > 9))
        {
            grid[gridCell.Coordinate] = 0;
            count++;
        }

        return count;
    }
}