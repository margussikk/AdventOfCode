using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2022.Day01;

[Puzzle(2022, 1, "Calorie Counting")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyCollection<Elf> _elves = [];

    public void ParseInput(string[] inputLines)
    {
        _elves = inputLines.SelectToChunks()
                           .Select(Elf.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _elves.Max(e => e.TotalCalories);

        return new PuzzleAnswer(answer, 69912);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _elves.Select(e => e.TotalCalories)
                           .OrderDescending()
                           .Take(3)
                           .Sum();

        return new PuzzleAnswer(answer, 208180);
    }
}
