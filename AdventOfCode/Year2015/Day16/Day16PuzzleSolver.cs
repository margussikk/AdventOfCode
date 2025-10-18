using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day16;

[Puzzle(2015, 16, "Aunt Sue")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<string, int> _tickerTape = new()
    {
        ["children"] = 3,
        ["cats"] = 7,
        ["samoyeds"] = 2,
        ["pomeranians"] = 3,
        ["akitas"] = 0,
        ["vizslas"] = 0,
        ["goldfish"] = 5,
        ["trees"] = 3,
        ["cars"] = 2,
        ["perfumes"] = 1
    };

    private IReadOnlyList<Sue> _sues = [];

    public void ParseInput(string[] inputLines)
    {
        _sues = [.. inputLines.Select(Sue.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _sues.First(sue => sue.Properties.All(kv => _tickerTape[kv.Key] == kv.Value)).Id;

        return new PuzzleAnswer(answer, 40);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _sues.First(sue =>
            sue.Properties.All(kv => kv.Key switch
            {
                "cats" or "trees" => kv.Value > _tickerTape[kv.Key],
                "pomeranians" or "goldfish" => kv.Value < _tickerTape[kv.Key],
                _ => _tickerTape[kv.Key] == kv.Value,
            })).Id;

        return new PuzzleAnswer(answer, 241);
    }
}