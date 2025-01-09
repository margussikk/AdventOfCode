using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2021.Day15;

[Puzzle(2021, 15, "Chiton")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private Grid<byte> _riskLevelGrid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _riskLevelGrid = inputLines.SelectToGrid(character => Convert.ToByte(character.ParseToDigit()));
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var pathFinder = new GridPathFinder<byte>(_riskLevelGrid)
            .SetCostCalculator((w, c) => w.Cost + _riskLevelGrid[c.Coordinate]);

        var answer = pathFinder.FindShortestPathLength(GridCoordinate.Zero, new GridCoordinate(_riskLevelGrid.LastRow, _riskLevelGrid.LastColumn));

        return new PuzzleAnswer(answer, 717);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var grid = new Grid<byte>(_riskLevelGrid.Height * 5, _riskLevelGrid.Width * 5);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                var value = _riskLevelGrid[row % _riskLevelGrid.Height, column % _riskLevelGrid.Width] +
                            row / _riskLevelGrid.Height +
                            column / _riskLevelGrid.Width;
                value = value > 9 ? value % 10 + 1 : value;

                grid[row, column] = Convert.ToByte(value);
            }
        }

        var pathFinder = new GridPathFinder<byte>(grid)
            .SetCostCalculator((w, c) => w.Cost + grid[c.Coordinate]);

        var answer = pathFinder.FindShortestPathLength(GridCoordinate.Zero, new GridCoordinate(grid.LastRow, grid.LastColumn));


        return new PuzzleAnswer(answer, 2993);
    }
}