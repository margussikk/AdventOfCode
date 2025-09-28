using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2016.Day06;

[Puzzle(2016, 6, "Signals and Noise")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private string[] _inputLines = [];

    public void ParseInput(string[] inputLines)
    {
        _inputLines = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var frequencyTables = BuildFrequencyTables();

        var answer = new string([.. frequencyTables.Select(table => table.MaxBy(kvp => kvp.Value).Key)]);

        return new PuzzleAnswer(answer, "qrqlznrl");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var frequencyTables = BuildFrequencyTables();

        var answer = new string([.. frequencyTables.Select(table => table.MinBy(kvp => kvp.Value).Key)]);

        return new PuzzleAnswer(answer, "kgzdfaon");
    }

    private List<Dictionary<char, int>> BuildFrequencyTables()
    {
        var frequencyTables = Enumerable.Range(0, _inputLines[0].Length)
                                        .Select(x => new Dictionary<char, int>())
                                        .ToList();

        foreach (var line in _inputLines)
        {
            for (var characterIndex = 0; characterIndex < line.Length; characterIndex++)
            {
                frequencyTables[characterIndex].IncrementValue(line[characterIndex], 1);
            }
        }

        return frequencyTables;
    }
}