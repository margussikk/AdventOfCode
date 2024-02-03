using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2020.Day06;

[Puzzle(2020, 6, "Custom Customs")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private List<AnswerGroup> _answerGroups = [];

    public void ParseInput(string[] inputLines)
    {
        _answerGroups = inputLines.SelectToChunks()
                                  .Select(AnswerGroup.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _answerGroups.Sum(ag => ag.CountAnyoneAnswered());

        return new PuzzleAnswer(answer, 7110);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _answerGroups.Sum(ag => ag.CountEveryoneAnswered());

        return new PuzzleAnswer(answer, 3628);
    }
}