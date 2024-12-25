using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2024.Day05;

[Puzzle(2024, 5, "Print Queue")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private ILookup<int, int> _pageOrderingRules = Array.Empty<int>().ToLookup(kvp => kvp);
    private List<int[]> _updates = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _pageOrderingRules = chunks[0].Select(line => line.SplitToNumbers<int>('|'))
                                      .ToLookup(numbers => numbers[0], numbers => numbers[1]);

        _updates = chunks[1].Select(line => line.SplitToNumbers<int>(','))
                            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _updates.Where(IsCorrectlyOrdered)
                             .Sum(GetMiddlePageNumber);

        return new PuzzleAnswer(answer, 5329);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var comparer = new PageNumberComparer(_pageOrderingRules);

        var answer = _updates.Where(update => !IsCorrectlyOrdered(update))
                             .Select(update => update.Order(comparer).ToArray())
                             .Sum(GetMiddlePageNumber);

        return new PuzzleAnswer(answer, 5833);
    }

    public bool IsCorrectlyOrdered(int[] update)
    {
        return Enumerable.Range(0, update.Length)
                         .All(index => !update[(index + 1)..].Except(_pageOrderingRules[update[index]]).Any());
    }

    private static int GetMiddlePageNumber(int[] update)
    {
        return update[update.Length / 2];
    }
}