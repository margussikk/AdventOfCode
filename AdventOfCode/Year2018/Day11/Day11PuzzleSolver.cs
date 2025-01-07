using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2018.Day11;

[Puzzle(2018, 11, "Chronal Charge")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private int _serialNumber;

    private SummedAreaTable _summedAreaTable = new(new(0, 0));

    public void ParseInput(string[] inputLines)
    {
        _serialNumber = int.Parse(inputLines[0]);

        var matrix = new Matrix<int>(300, 300);

        for (var row = 0; row <= matrix.LastRowIndex; row++)
        {
            for (var column = 0; column <= matrix.LastColumnIndex; column++)
            {
                matrix[row, column] = CalculatePowerLevel(row, column);
            }
        }

        _summedAreaTable = new SummedAreaTable(matrix);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var result = FindTotalPower(3);

        var answer = $"{result.Coordinate.Column + 1},{result.Coordinate.Row + 1}";

        return new PuzzleAnswer(answer, "20,62");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var bestResult = new TotalPowerResult(GridCoordinate.Zero, 0, int.MinValue);

        for (var squareSize = 1; squareSize <= 300; squareSize++)
        {
            var result = FindTotalPower(squareSize);
            if (result.TotalPower > bestResult.TotalPower)
            {
                bestResult = result;
            }
        }

        var answer = $"{bestResult.Coordinate.Column + 1},{bestResult.Coordinate.Row + 1},{bestResult.SquareSize}";

        return new PuzzleAnswer(answer, "229,61,16");
    }

    private TotalPowerResult FindTotalPower(int squareSize)
    {
        var largestTotalPower = int.MinValue;
        var largestTotalPowerCoordinate = GridCoordinate.Zero;

        for (var row = 0; row <= _summedAreaTable.Height - squareSize; row++)
        {
            for (var column = 0; column <= _summedAreaTable.Width - squareSize; column++)
            {
                var sum = _summedAreaTable.GetRectangleSum(row, column, row + squareSize - 1, column + squareSize - 1);
                if (sum > largestTotalPower)
                {
                    largestTotalPower = sum;
                    largestTotalPowerCoordinate = new GridCoordinate(row, column);
                }
            }
        }

        return new TotalPowerResult(largestTotalPowerCoordinate, squareSize, largestTotalPower);
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

    private sealed record TotalPowerResult(GridCoordinate Coordinate, int SquareSize, int TotalPower);
}