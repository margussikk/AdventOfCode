using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day01;

[Puzzle(2017, 1, "Inverse Captcha")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private List<int> _input = [];

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0].Select(c => c.ParseToDigit()).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(1);

        return new PuzzleAnswer(answer, 1102);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(_input.Count / 2);

        return new PuzzleAnswer(answer, 1076);
    }

    private int GetAnswer(int offset)
    {
        var answer = 0;

        for (var index = 0; index < _input.Count; index++)
        {
            var first = _input[index];
            var second = _input[(index + offset) % _input.Count];

            if (first == second)
            {
                answer += first;
            }
        }

        return answer;
    }
}