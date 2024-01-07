using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day09;

[Puzzle(2023, 9, "Mirage Maintenance")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private List<History> _histories = [];

    public void ParseInput(List<string> inputLines)
    {
        _histories = inputLines.Select(History.Parse)
                               .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _histories.Sum(history => history.GetNextValue());

        return new PuzzleAnswer(answer, 1681758908L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _histories.Sum(history => history.GetPreviousValue());

        return new PuzzleAnswer(answer, 803);
    }
}
