using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2021.Day14;

[Puzzle(2021, 14, "Extended Polymerization")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private string _polymer = string.Empty;

    private readonly Dictionary<CharacterPair, char> _insertions = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _polymer = chunks[0][0];

        foreach (var line in chunks[1])
        {
            var splits = line.Split(" -> ");

            _insertions.Add(new CharacterPair(splits[0][0], splits[0][1]), splits[1][0]);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(10);

        return new PuzzleAnswer(answer, 2223);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(40);

        return new PuzzleAnswer(answer, 2566282754493L);
    }

    private long GetAnswer(int steps)
    {
        var letterCounts = new long['Z' - 'A' + 1];
        var pairCounts = new Dictionary<CharacterPair, long>();

        for (var i = 0; i < _polymer.Length - 1; i++)
        {
            var pair = new CharacterPair(_polymer[i], _polymer[i + 1]);

            var pairCount = pairCounts.GetValueOrDefault(pair, 0);
            pairCounts[pair] = pairCount + 1;

            letterCounts[_polymer[i] - 'A']++;
        }

        letterCounts[_polymer[^1] - 'A']++;

        for (var step = 0; step < steps; step++)
        {
            var newPairCounts = new Dictionary<CharacterPair, long>();

            foreach (var kvp in pairCounts)
            {
                var inserted = _insertions[kvp.Key];

                var left = new CharacterPair(kvp.Key.Left, inserted);
                newPairCounts[left] = newPairCounts.GetValueOrDefault(left, 0) + kvp.Value;

                var right = new CharacterPair(inserted, kvp.Key.Right);
                newPairCounts[right] = newPairCounts.GetValueOrDefault(right, 0) + kvp.Value;

                letterCounts[inserted - 'A'] += kvp.Value;
            }

            pairCounts = newPairCounts;
        }

        var minCount = letterCounts.Where(x => x > 0).Min();
        var maxCount = letterCounts.Where(x => x > 0).Max();

        return maxCount - minCount;
    }
}