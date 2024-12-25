using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2024.Day01;

[Puzzle(2024, 1, "Historian Hysteria")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private readonly List<int> _leftLocationIds = [];
    private readonly List<int> _rightLocationIds = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var numbers = line.SplitToNumbers<int>(' ');

            _leftLocationIds.Add(numbers[0]);
            _rightLocationIds.Add(numbers[1]);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _leftLocationIds
            .OrderBy(x => x)
            .Zip(_rightLocationIds.OrderBy(x => x))
            .Sum(x => Math.Abs(x.First - x.Second));

        return new PuzzleAnswer(answer, 1873376);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var occurencies = _rightLocationIds.ToLookup(x => x);

        var answer = _leftLocationIds.Sum(x => x * occurencies[x].Count());

        return new PuzzleAnswer(answer, 18997088);
    }
}