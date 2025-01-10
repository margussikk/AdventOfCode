using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day02;

[Puzzle(2017, 2, "Corruption Checksum")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private List<int[]> _spreadsheet = [];

    public void ParseInput(string[] inputLines)
    {
        _spreadsheet = inputLines.Select(line => line.SplitToNumbers<int>('\t'))
                                 .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _spreadsheet.Sum(row => row.Max() - row.Min());

        return new PuzzleAnswer(answer, 30994);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _spreadsheet.Sum(row => row.OrderDescending()
                                                .Pairs()
                                                .Where(x => x.First % x.Second == 0)
                                                .Select(x => x.First / x.Second)
                                                .First());

        return new PuzzleAnswer(answer, 233);
    }
}