using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day06;

[Puzzle(2021, 6, "Lanternfish")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private List<int> _lanternfishTimers = [];

    public void ParseInput(string[] inputLines)
    {
        _lanternfishTimers = inputLines[0]
            .Split(",")
            .Select(int.Parse)
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(80);

        return new PuzzleAnswer(answer, 360610);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(256);

        return new PuzzleAnswer(answer, 1631629590423L);
    }

    private long GetAnswer(int days)
    {
        var internalTimers = new long[9]; // 0..8

        foreach (var lanternfishTimers in _lanternfishTimers)
        {
            internalTimers[lanternfishTimers]++;
        }

        for (var day = 0; day < days; day++)
        {
            var newFish = internalTimers[0];

            for (int i = 0; i < 8; i++)
            {
                internalTimers[i] = internalTimers[i + 1];
            }

            internalTimers[6] += newFish;
            internalTimers[8] = newFish;
        }

        return internalTimers.Sum();
    }
}