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

        var answer = frequencyTables.Select(table => table.MaxBy(kvp => kvp.Value).Key).JoinToString();

        return new PuzzleAnswer(answer, "qrqlznrl");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var frequencyTables = BuildFrequencyTables();

        var answer = frequencyTables.Select(table => table.MinBy(kvp => kvp.Value).Key).JoinToString();

        return new PuzzleAnswer(answer, "kgzdfaon");
    }

    private List<Dictionary<char, int>> BuildFrequencyTables()
    {
        var frequencyTables = Enumerable.Range(0, _inputLines[0].Length)
                                        .Select(column => _inputLines.Select(line => line[column])
                                                                     .GroupBy(x => x)
                                                                     .ToDictionary(x => x.Key, x => x.Count()))
                                        .ToList();

        return frequencyTables;
    }
}