using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day02;

[Puzzle(2020, 2, "Password Philosophy")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private List<PasswordPolicy> _passwordPolicies = [];

    public void ParseInput(string[] inputLines)
    {
        _passwordPolicies = inputLines.Select(PasswordPolicy.Parse)
                                      .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        foreach (var policy in _passwordPolicies)
        {
            var characterCount = policy.Password.Count(character => character == policy.Character);
            if (policy.FirstNumber <= characterCount && characterCount <= policy.SecondNumber)
            {
                answer++;
            }
        }

        return new PuzzleAnswer(answer, 564);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        foreach (var policy in _passwordPolicies)
        {
            var indexes = new[]
            {
                policy.FirstNumber - 1,
                policy.SecondNumber - 1
            };

            var matches = indexes.Count(index =>
                index < policy.Password.Length &&
                policy.Password[index] == policy.Character);

            if (matches == 1)
            {
                answer++;
            }
        }

        return new PuzzleAnswer(answer, 325);
    }
}