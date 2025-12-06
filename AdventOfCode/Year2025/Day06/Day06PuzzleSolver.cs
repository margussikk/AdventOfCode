using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2025.Day06;

[Puzzle(2025, 6, "Trash Compactor")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Problem> _problems = [];

    public void ParseInput(string[] inputLines)
    {
        // NB! Assume each line has the same length

        var problems = new List<Problem>()
        {
            new()
        };

        for (var index = 0; index < inputLines[0].Length; index++)
        {
            var characters = inputLines.Select(line => line[index]).ToList();

            if (characters.All(x => x == ' '))
            {
                problems.Add(new Problem());

                continue;
            }

            var problem = problems[^1];

            problem.Numbers = [.. characters.Take(characters.Count - 1)
                                            .Select((c, i) => $"{(i < problem.Numbers.Count ? problem.Numbers[i] : string.Empty)}{c}")];
            if (characters[^1] == '+')
            {
                problem.Operation = ProblemOperation.Add;
            }
            else if (characters[^1] == '*')
            {
                problem.Operation = ProblemOperation.Multiply;
            }
        }

        _problems = problems;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _problems.Sum(p => p.Solve(1));

        return new PuzzleAnswer(answer, 4878670269096L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _problems.Sum(p => p.Solve(2));

        return new PuzzleAnswer(answer, 8674740488592L);
    }
}