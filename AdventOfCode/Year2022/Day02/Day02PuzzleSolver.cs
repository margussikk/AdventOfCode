using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day02;

[Puzzle(2022, 2, "Rock Paper Scissors")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Round> _rounds = [];

    public void ParseInput(string[] inputLines)
    {
        _rounds = inputLines.Select(Round.Parse)
                            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _rounds.Sum(r => r.CalculateScorePartOne());

        return new PuzzleAnswer(answer, 10941);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _rounds.Sum(r => r.CalculateScorePartTwo());

        return new PuzzleAnswer(answer, 13071);
    }
}
