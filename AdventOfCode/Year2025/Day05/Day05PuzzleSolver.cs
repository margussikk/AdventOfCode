using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2025.Day05;

[Puzzle(2025, 5, "Cafeteria")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<NumberRange<long>> _idRanges = [];
    private IReadOnlyList<long> _ids = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _idRanges = [.. chunks[0].Select(NumberRange<long>.Parse)];
        _ids = [.. chunks[1].Select(long.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _ids.Count(id => _idRanges.Any(range => range.Contains(id)));

        return new PuzzleAnswer(answer, 643);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = NumberRange<long>.Merge([.. _idRanges])
                                      .Sum(range => range.Length);

        return new PuzzleAnswer(answer, 342018167474526L);
    }
}