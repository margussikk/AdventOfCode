using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day25;

// https://en.wikipedia.org/wiki/Diffie%E2%80%93Hellman_key_exchange

[Puzzle(2020, 25, "Combo Breaker")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private List<int> _publicKeys = [];

    public void ParseInput(string[] inputLines)
    {
        _publicKeys = inputLines.Select(int.Parse)
                                .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        FindLoopSizes(_publicKeys[0], _publicKeys[1], out var loopSize1, out var loopSize2);

        var answer = 0L;

        if (loopSize1.HasValue)
        {
            answer = TransformSubjectNumber(_publicKeys[1], loopSize1.Value);
        }
        else if (loopSize2.HasValue)
        {
            answer = TransformSubjectNumber(_publicKeys[0], loopSize2.Value);
        }

        return new PuzzleAnswer(answer, 545789);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }

    private static void FindLoopSizes(int publicKey1, int publicKey2, out int? loopSize1, out int? loopSize2)
    {
        loopSize1 = null;
        loopSize2 = null;

        var power = 0;
        var value = 1;

        while (!loopSize1.HasValue && !loopSize2.HasValue)
        {
            power++;
            value *= 7;
            value %= 20201227;

            if (value == publicKey1)
            {
                loopSize1 = power;
            }
            else if (value == publicKey2)
            {
                loopSize2 = power;
            }
        }
    }

    private static long TransformSubjectNumber(int subjectNumber, int loopSize)
    {
        var value = 1L;

        for (var loop = 0; loop < loopSize; loop++)
        {
            value *= subjectNumber;
            value %= 20201227;
        }

        return value;
    }
}