using AdventOfCode.Framework.Puzzle;
using CommandLine;

namespace AdventOfCode.Year2016.Day21;

[Puzzle(2016, 21, "Scrambled Letters and Hash")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Operation> _operations = [];

    public void ParseInput(string[] inputLines)
    {
        _operations = [.. inputLines.Select(Operation.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var letters = "abcdefgh".ToArray();

        foreach (var operation in _operations)
        {
            operation.Scramble(letters);
        }

        var answer = new string(letters);

        return new PuzzleAnswer(answer, "gbhcefad");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var letters = "fbgdceah".ToArray();

        foreach (var operation in _operations.Reverse())
        {
            operation.Unscramble(letters);
        }

        var answer = new string(letters);

        return new PuzzleAnswer(answer, "gahedfcb");
    }
}