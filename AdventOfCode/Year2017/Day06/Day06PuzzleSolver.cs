using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day06;

[Puzzle(2017, 6, "Memory Reallocation")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private int[] _memoryBanks = [];

    public void ParseInput(string[] inputLines)
    {
        _memoryBanks = inputLines[0].SplitToNumbers<int>('\t');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var seenMemoryBanks = new HashSet<int>();

        var memoryBanks = _memoryBanks.ToArray();

        while (seenMemoryBanks.Add(memoryBanks.GetSequenceHashCode()))
        {
            Redistribute(memoryBanks);
        }

        return new PuzzleAnswer(seenMemoryBanks.Count, 4074);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var memoryBankCycleStarts = new Dictionary<int, int>();

        var memoryBanks = _memoryBanks.ToArray();

        while (memoryBankCycleStarts.TryAdd(memoryBanks.GetSequenceHashCode(), memoryBankCycleStarts.Count))
        {
            Redistribute(memoryBanks);
        }

        var answer = memoryBankCycleStarts.Count - memoryBankCycleStarts[memoryBanks.GetSequenceHashCode()];

        return new PuzzleAnswer(answer, 2793);
    }

    private static void Redistribute(int[] memoryBanks)
    {
        var max = memoryBanks.Max();
        var memoryBank = Array.IndexOf(memoryBanks, max);

        var blocks = memoryBanks[memoryBank];
        memoryBanks[memoryBank] = 0;
        while (blocks > 0)
        {
            memoryBank = (memoryBank + 1) % memoryBanks.Length;
            memoryBanks[memoryBank]++;

            blocks--;
        }
    }
}