using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2022.Day16;
using System;
using System.Security.Cryptography;

namespace AdventOfCode.Year2024.Day02;

[Puzzle(2024, 2, "Red-Nosed Reports")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private List<int[]> _records = [];

    public void ParseInput(string[] inputLines)
    {
        _records = inputLines.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(int.Parse)
                                           .ToArray())
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

    private static bool IsSafe(int[] record)
    {
        if (record[1] == record[0])
        {
            return false;
        }

        Func<int, int, int> Diff = record[1] > record[0]
            ? (r1, r2) => r2 - r1
            : (r1, r2) => r1 - r2;

        for (var i = 0; i < record.Length - 1; i++)
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
        var isSafe = IsSafe(record);
        if (isSafe)
        {
            return true;
        }

        for (var index = 0; index < record.Length; index++)
        {
            var newRecord = record.Where((v, i) => i != index)
                                  .ToArray();

            isSafe = IsSafe(newRecord);
            if (isSafe)
            {
                return true;
            }
        }

        return false;
    }
}
