using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2025.Day06;

internal class Problem
{
    public IReadOnlyList<string> Numbers { get; set; } = [];
    public ProblemOperation Operation { get; set; }

    public long Solve(int part)
    {
        var numbers = part == 1
            ? Numbers.Select(long.Parse)
            : Enumerable.Range(0, Numbers[0].Length)
                        .Select(i => Numbers.Select(n => n[i]).JoinToString().Trim())
                        .Select(long.Parse);

        return Operation switch
        {
            ProblemOperation.Add => numbers.Sum(),
            ProblemOperation.Multiply => numbers.Aggregate(1L, (acc, x) => acc * x),
            _ => throw new InvalidOperationException($"Unsupported operation {Operation}")
        };
    }
}
