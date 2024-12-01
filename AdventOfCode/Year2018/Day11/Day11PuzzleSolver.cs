using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day11;

[Puzzle(2018, 11, "Chronal Charge")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private int _serialNumber;

    private Grid<int> _grid = new Grid<int>(300, 300);

    public void ParseInput(string[] inputLines)
    {
        _serialNumber = int.Parse(inputLines[0]);

        for (var row = 0; row <= _grid.LastRowIndex; row++)
        {
            for (var column = 0; column <= _grid.LastColumnIndex; column++)
            {
                _grid[row, column] = CalculatePowerLevel(row, column);
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var result = GetLargestTotalPowerResultFixed(3);

        var answer = $"{result.Coordinate.Column + 1},{result.Coordinate.Row + 1}";

        return new PuzzleAnswer(answer, "20,62");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var result = GetLargestTotalPowerResultDynamic();

        var answer = $"{result.Coordinate.Column + 1},{result.Coordinate.Row + 1},{result.Size}";

        return new PuzzleAnswer(answer, "229,61,16");
    }

    private TotalPowerResult GetLargestTotalPowerResultFixed(int size)
    {
        var result = new TotalPowerResult(new GridCoordinate(0, 0), size, int.MinValue);

        for (var outerRow = 0; outerRow <= _grid.LastRowIndex - size; outerRow++)
        {
            for (var outerColumn = 0; outerColumn <= _grid.LastColumnIndex - size; outerColumn++)
            {
                var totalPower = 0;

                for (var innerRow = outerRow; innerRow < outerRow + size; innerRow++)
                {
                    for (var innerColumn = outerColumn; innerColumn < outerColumn + size; innerColumn++)
                    {
                        totalPower += _grid[innerRow, innerColumn];
                    }
                }

                if (totalPower > result.TotalPower)
                {
                    var coordinate = new GridCoordinate(outerRow, outerColumn);
                    result = new TotalPowerResult(coordinate, size, totalPower);
                }
            }
        }

        return result;
    }

    private TotalPowerResult GetLargestTotalPowerResultDynamic()
    {
        var previousSizeGrid = _grid;

        var cell = previousSizeGrid.MaxBy(x => x.Object);
        var result = new TotalPowerResult(cell.Coordinate, 1, cell.Object);

        for (var size = 2; size <= 300; size++)
        {
            var currentSizeGrid = new Grid<int>(300, 300);

            for (var outerRow = 0; outerRow <= _grid.LastRowIndex - size; outerRow++)
            {
                for (var outerColumn = 0; outerColumn <= _grid.LastColumnIndex - size; outerColumn++)
                {
                    var previousSize = size - 1;

                    var coordinate = new GridCoordinate(outerRow, outerColumn);

                    var previousSizeSquarePower = previousSizeGrid[coordinate];

                    var expansionRowPower = Enumerable.Range(outerColumn, previousSize)
                                                      .Sum(c => _grid[outerRow + previousSize, c]);

                    var expansionColumnPower = Enumerable.Range(outerRow, previousSize)
                                                         .Sum(r => _grid[r, outerColumn + previousSize]);

                    var totalPower = previousSizeSquarePower + expansionRowPower + expansionColumnPower + _grid[outerRow + previousSize, outerColumn + previousSize];

                    currentSizeGrid[coordinate] = totalPower;

                    if (totalPower > result.TotalPower)
                    {
                        result = new TotalPowerResult(coordinate, size, totalPower);
                    }
                }
            }

            previousSizeGrid = currentSizeGrid;
        }

        return result;
    }

    private int CalculatePowerLevel(int row, int column)
    {
        var x = column + 1;
        var y = row + 1;

        var rackId = x + 10;
        var powerLevel = rackId * y;
        powerLevel += _serialNumber;
        powerLevel *= rackId;

        var hundredsDigit = (powerLevel / 100) % 10;
        return hundredsDigit - 5;
    }

    private sealed record TotalPowerResult(GridCoordinate Coordinate, int Size, int TotalPower);
}