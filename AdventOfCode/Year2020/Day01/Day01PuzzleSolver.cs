using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day01;

[Puzzle(2020, 1, "Report Repair")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private List<int> _entries = [];

    public void ParseInput(string[] inputLines)
    {
        _entries = inputLines.Select(int.Parse)
                             .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var orderedEntries = _entries.Order()
                                     .ToList();

        GetAnswer(orderedEntries, 2, 0, 0, out var answer);

        return new PuzzleAnswer(answer, 1007104);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var orderedEntries = _entries.Order()
                                     .ToList();

        GetAnswer(orderedEntries, 3, 0, 0, out var answer);

        return new PuzzleAnswer(answer, 18847752);
    }

    private static bool GetAnswer(List<int> entries, int summandCount, int startIndex, int currentSum, out long answer)
    {
        for (var index = startIndex; index < entries.Count - summandCount + 1; index++)
        {
            var entry = entries[index];

            if (currentSum + entry > 2020)
            {
                answer = 0L;
                return false;
            }

            if (summandCount == 1)
            {
                if (currentSum + entry != 2020) continue;
                
                answer = entry;
                return true;
            }

            var found = GetAnswer(entries, summandCount - 1, index + 1, currentSum + entry, out var answer1);
            if (!found) continue;
            
            answer = entry * answer1;
            return found;
        }

        answer = 0L;
        return false;
    }
}