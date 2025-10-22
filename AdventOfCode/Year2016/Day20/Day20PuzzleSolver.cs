using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2016.Day20;

[Puzzle(2016, 20, "Firewall Rules")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<NumberRange<uint>> _ips = [];

    public void ParseInput(string[] inputLines)
    {
        _ips = [.. inputLines
            .Select(x =>
            {
                var numbers = x.Replace('-', ' ').SplitToNumbers<uint>(' ');
                return new NumberRange<uint>(numbers[0], numbers[1]);
            })];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var mergedIps = NumberRange<uint>.Merge([.. _ips]);

        var answer = mergedIps[0].Start == 0 ? mergedIps[0].End + 1 : 0;

        return new PuzzleAnswer(answer, 23923783U);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var mergedIps = NumberRange<uint>.Merge([.. _ips]);

        var answer = uint.MaxValue - mergedIps.Sum(x => x.Length) + 1; // + 1 to include ip 0

        return new PuzzleAnswer(answer, 125);
    }
}