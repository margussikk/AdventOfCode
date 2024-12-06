using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2024.Day05;

[Puzzle(2024, 5, "Print Queue")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private ILookup<int, int> _pageOrderingRules = Array.Empty<int>().ToLookup(kvp => kvp);
    private List<List<int>> _updates = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _pageOrderingRules = chunks[0].Select(line => line.Split('|'))
                                      .ToLookup(splits => int.Parse(splits[0]), splits => int.Parse(splits[1]));

        _updates = chunks[1].Select(line => line.Split(',')
                                                .Select(int.Parse)
                                                .ToList())
                            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _updates.Where(IsCorrectlyOrdered)
                             .Sum(x => x[x.Count / 2]);

        return new PuzzleAnswer(answer, 5329);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var comparer = new PageNumberComparer(_pageOrderingRules);

        var answer = _updates.Where(x => !IsCorrectlyOrdered(x))
                             .Select(x => x.OrderBy(x => x, comparer).ToList())
                             .Sum(x => x[x.Count / 2]);

        return new PuzzleAnswer(answer, 5833);
    }

    public bool IsCorrectlyOrdered(List<int> update)
    {
        return update.Select((pageNumber1, index) => update.Skip(index + 1).All(pageNumber2 => _pageOrderingRules[pageNumber1].Contains(pageNumber2)))
                     .All(x => x);
    }
}