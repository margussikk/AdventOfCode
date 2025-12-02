using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2025.Day02;

[Puzzle(2025, 2, "Gift Shop")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<NumberRange<long>> _ranges = [];

    public void ParseInput(string[] inputLines)
    {
        _ranges = [.. inputLines[0].Split(',').Select(NumberRange<long>.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0L;

        foreach (var id in _ranges.SelectMany(x => x))
        {
            var idString = id.ToString();
            if (idString.Length % 2 != 0)
            {
                continue;
            }

            var length = idString.Length / 2;
            if (IsInvalid(idString, length))
            {
                answer += id;
            }
        }

        return new PuzzleAnswer(answer, 26255179562L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0L;

        foreach (var id in _ranges.SelectMany(x => x))
        {
            var idString = id.ToString();

            for (var length = 1; length <= idString.Length / 2; length++)
            {
                if (idString.Length % length != 0)
                {
                    continue;
                }

                if (IsInvalid(idString, length))
                {
                    answer += id;
                    break;
                }
            }
        }


        return new PuzzleAnswer(answer, 31680313976L);
    }

    private static bool IsInvalid(string idString, int length)
    {
        for (var charIndex = 0; charIndex < length; charIndex++)
        {
            for (var partIndex = 0; partIndex < idString.Length / length; partIndex++)
            {
                if (idString[partIndex * length + charIndex] != idString[charIndex])
                {
                    return false;
                }
            }
        }

        return true;
    }
}