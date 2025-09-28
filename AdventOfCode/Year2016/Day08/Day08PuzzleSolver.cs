using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Text;
using System.Text;

namespace AdventOfCode.Year2016.Day08;

[Puzzle(2016, 8, "Two-Factor Authentication")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines.Select(Instruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = new Grid<bool>(6, 50);

        foreach (var instruction in _instructions)
        {
            ApplyInstruction(grid, instruction);
        }

        var answer = grid.Count(cell => cell.Object);

        return new PuzzleAnswer(answer, 119);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var grid = new Grid<bool>(6, 50);

        foreach (var instruction in _instructions)
        {
            ApplyInstruction(grid, instruction);
        }

        var text = grid.PrintToString(x => x ? '#' : ' ');
        var answer = Ocr.Parse(text);

        return new PuzzleAnswer(answer, "ZFHFSFOGPO");
    }

    private static void ApplyInstruction(Grid<bool> grid, Instruction instruction)
    {
        switch (instruction)
        {
            case RectInstruction rectInstruction:
                for (var row = 0; row < rectInstruction.Height; row++)
                {
                    for (var column = 0; column < rectInstruction.Width; column++)
                    {
                        grid[row, column] = true;
                    }
                }
                break;
            case RotateRowInstruction rotateRowInstruction:
                var newRow = new bool[grid.Width];
                for (var column = 0; column < grid.Width; column++)
                {
                    newRow[(column + rotateRowInstruction.Shift) % grid.Width] = grid[rotateRowInstruction.Row, column];
                }
                for (var column = 0; column < grid.Width; column++)
                {
                    grid[rotateRowInstruction.Row, column] = newRow[column];
                }
                break;
            case RotateColumnInstruction rotateColumnInstruction:
                var newColumn = new bool[grid.Height];
                for (var row = 0; row < grid.Height; row++)
                {
                    newColumn[(row + rotateColumnInstruction.Shift) % grid.Height] = grid[row, rotateColumnInstruction.Column];
                }
                for (var row = 0; row < grid.Height; row++)
                {
                    grid[row, rotateColumnInstruction.Column] = newColumn[row];
                }
                break;
            default:
                throw new InvalidOperationException("Invalid instruction");
        }
    }
}