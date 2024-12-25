using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2024.Day25;

[Puzzle(2024, 25, "Code Chronicle")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private readonly List<int[]> _locks = [];
    private readonly List<int[]> _keys = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        foreach (var chunk in chunks)
        {
            var pins = new int[5]; // Assume it's always 5 pins
            foreach (var line in chunk)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] == '#')
                    {
                        pins[i]++;
                    }
                }
            }

            if (chunk[0][0] == '#')
            {
                _locks.Add(pins);
            }
            else
            {
                _keys.Add(pins);
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _keys.Sum(key => _locks.Count(@lock => key.Zip(@lock)
                                                               .All(pair => pair.First + pair.Second <= 7)));

        return new PuzzleAnswer(answer, 3090);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}