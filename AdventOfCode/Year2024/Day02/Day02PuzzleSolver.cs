using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2024.Day02;

[Puzzle(2024, 2, "Red-Nosed Reports")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private List<int[]> _records = [];

    public void ParseInput(string[] inputLines)
    {
        _records = inputLines.Select(x => x.SplitToNumbers<int>(' '))
                             .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _records.Count(IsSafe);

        return new PuzzleAnswer(answer, 299);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _records.Count(IsSafeUsingProblemDampener);

        return new PuzzleAnswer(answer, 364);
    }

    private static bool IsSafe(IList<int> record)
    {
        if (record[1] == record[0])
        {
            return false;
        }

        Func<int, int, int> Diff = record[1] > record[0]
            ? (r1, r2) => r2 - r1
            : (r1, r2) => r1 - r2;

        for (var i = 0; i < record.Count - 1; i++)
        {
            var diff = Diff(record[i], record[i + 1]);
            if (diff < 1 || diff > 3)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSafeUsingProblemDampener(int[] record)
    {
        for (var index = 0; index < record.Length; index++)
        {
            var newRecord = new List<int>(record);
            newRecord.RemoveAt(index);

            if (IsSafe(newRecord))
            {
                return true;
            }
        }

        return false;
    }
}
