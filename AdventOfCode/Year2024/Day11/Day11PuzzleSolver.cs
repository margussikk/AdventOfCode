using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2024.Day11;

[Puzzle(2024, 11, "Plutonian Pebbles")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _stones = [];

    public void ParseInput(string[] inputLines)
    {
        _stones = inputLines[0].SplitToNumbers<long>(' ');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CountBlinkedStones(25);

        return new PuzzleAnswer(answer, 228668);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = CountBlinkedStones(75);

        return new PuzzleAnswer(answer, 270673834779359L);
    }

    private long CountBlinkedStones(int totalBlinks)
    {
        var stoneCounts = _stones.ToLookup(x => x)
                                 .ToDictionary(x => x.Key, x => x.LongCount());

        for (var blinkCounter = 0; blinkCounter < totalBlinks; blinkCounter++)
        {
            var newStoneCounts = new Dictionary<long, long>();

            foreach (var stoneCountKvp in stoneCounts)
            {
                foreach (var newStone in TransformStone(stoneCountKvp.Key))
                {
                    var count = newStoneCounts.GetValueOrDefault(newStone, 0);
                    newStoneCounts[newStone] = count + stoneCountKvp.Value;
                }
            }

            stoneCounts = newStoneCounts;
        }

        return stoneCounts.Sum(x => x.Value);
    }

    private static long[] TransformStone(long stone)
    {
        if (stone == 0)
        {
            return [1];
        }

        var digitCount = stone.DigitCount();
        if (digitCount % 2 == 0)
        {
            var divider = MathFunctions.Power10(digitCount / 2);
            return [stone / divider, stone % divider];
        }

        return [stone * 2024];
    }
}