using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Text;

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
            instruction.Execute(grid);
        }

        var answer = grid.Count(cell => cell.Object);

        return new PuzzleAnswer(answer, 119);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var grid = new Grid<bool>(6, 50);

        foreach (var instruction in _instructions)
        {
            instruction.Execute(grid);
        }

        var text = grid.PrintToString(x => x ? '#' : ' ');
        var answer = Ocr.Parse(text);

        return new PuzzleAnswer(answer, "ZFHFSFOGPO");
    }
}