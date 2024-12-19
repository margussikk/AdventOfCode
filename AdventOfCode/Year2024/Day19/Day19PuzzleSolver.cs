using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2024.Day19;

[Puzzle(2024, 19, "Linen Layout")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private List<string> _patterns = [];
    private string[] _designs = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _patterns = [.. chunks[0][0].Split(", ")];
        _designs = chunks[1];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var cache = new Dictionary<string, bool>();

        var answer = _designs.Count(x => IsPossible(cache, x));

        return new PuzzleAnswer(answer, 242);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cache = new Dictionary<string, long>();

        var answer = _designs.Sum(x => CountPossibilities(cache, x));

        return new PuzzleAnswer(answer, 595975512785325);
    }

    private bool IsPossible(Dictionary<string, bool> cache, string design)
    {
        if (design.Length == 0)
        {
            return true;
        }

        if (cache.TryGetValue(design, out var possible))
        {
            return possible;
        }

        possible = _patterns.Where(pattern => design.StartsWith(pattern, StringComparison.Ordinal))
                            .Any(pattern => IsPossible(cache, design[pattern.Length..]));

        cache[design] = possible;

        return possible;
    }

    private long CountPossibilities(Dictionary<string, long> cache, string design)
    {
        if (design.Length == 0)
        {
            return 1;
        }

        if (cache.TryGetValue(design, out var count))
        {
            return count;
        }

        count = _patterns.Where(pattern => design.StartsWith(pattern, StringComparison.Ordinal))
                         .Sum(pattern => CountPossibilities(cache, design[pattern.Length..]));

        cache[design] = count;

        return count;
    }
}