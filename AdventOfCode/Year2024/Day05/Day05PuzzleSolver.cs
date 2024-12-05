using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using System.Linq;

namespace AdventOfCode.Year2024.Day05;

[Puzzle(2024, 5, "Print Queue")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private ILookup<int, int> _pageOrderingRules = new Dictionary<int, int>().ToLookup(kvp => kvp.Key, kvp => kvp.Value);
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
        var answer = _updates.Where(x => !IsCorrectlyOrdered(x))
                             .Select(x => FixUpdate(x).FixedUpdate)
                             .Sum(x => x[x.Count / 2]);

        return new PuzzleAnswer(answer, 5833);
    }

    public bool IsCorrectlyOrdered(List<int> update)
    {
        return update.Select((pageNumber1, index) => update.Skip(index + 1).All(pageNumber2 => _pageOrderingRules[pageNumber1].Contains(pageNumber2)))
                     .All(x => x);
    }

    public (bool Success, List<int> FixedUpdate) FixUpdate(List<int> update)
    {
        if (update.Count == 1)
        {
            return (true, [update[0]]);
        }

        foreach (var pageNumber1 in update)
        {
            var otherPageNumbers = update.Where(x => x != pageNumber1).ToList();

            var isCorrect = otherPageNumbers.TrueForAll(pageNumber2 => _pageOrderingRules[pageNumber1].Contains(pageNumber2));
            if (!isCorrect)
            {
                continue;
            }

            var (Success, FixedUpdate) = FixUpdate(otherPageNumbers);
            if (Success)
            {
                return (true, [pageNumber1, .. FixedUpdate]);
            }
        }

        return (false, []);
    }
}