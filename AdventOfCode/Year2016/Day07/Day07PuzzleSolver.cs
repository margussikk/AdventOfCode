using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2016.Day07;

[Puzzle(2016, 7, "Internet Protocol Version 7")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<IPAddress> _ipAddresses = [];

    public void ParseInput(string[] inputLines)
    {
        _ipAddresses = [.. inputLines.Select(IPAddress.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _ipAddresses.Count(ip => ip.SupportsTls());

        return new PuzzleAnswer(answer, 118);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _ipAddresses.Count(ip => ip.SupportsSsl());

        return new PuzzleAnswer(answer, 260);
    }
}