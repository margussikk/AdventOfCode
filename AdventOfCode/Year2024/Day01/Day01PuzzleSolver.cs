using AdventOfCode.Framework.Puzzle;

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
            var splits = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            _leftLocationIds.Add(int.Parse(splits[0]));
            _rightLocationIds.Add(int.Parse(splits[1]));
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
        var frequencies = _rightLocationIds.ToLookup(x => x);

        var answer = _leftLocationIds.Sum(x => x * frequencies[x].Count());

        return new PuzzleAnswer(answer, 18997088);
    }
}