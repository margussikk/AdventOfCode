using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2015.Day06;

[Puzzle(2015, 6, "Probably a Fire Hazard")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines.Select(Instruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = new Grid<bool>(1000, 1000);

        foreach (var instruction in _instructions)
        {
            instruction.ExecutePartOne(grid);
        }

        var answer = grid.Count(c => c.Object);

        return new PuzzleAnswer(answer, 543903);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var grid = new Grid<int>(1000, 1000);

        foreach (var instruction in _instructions)
        {
            instruction.ExecutePartTwo(grid);
        }

        var answer = grid.Sum(c => c.Object);

        return new PuzzleAnswer(answer, 14687245);
    }
}