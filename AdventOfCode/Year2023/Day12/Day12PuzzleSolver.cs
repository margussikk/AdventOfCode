using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day12;

[Puzzle(2023, 12, "Hot Springs")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private List<Record> _records = [];

    public void ParseInput(List<string> inputLines)
    {
        _records = inputLines
            .Select(Record.Parse)
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _records
            .Select(x => x.CountArrangementsV1())
            .Sum();

        return new PuzzleAnswer(answer, 7032);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _records
            .Select(x => x.Unfolded())
            .Select(x => x.CountArrangementsV1())
            .Sum();

        return new PuzzleAnswer(answer, 1493340882140L);
    }
}
