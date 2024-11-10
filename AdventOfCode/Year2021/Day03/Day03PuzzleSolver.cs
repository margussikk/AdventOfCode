using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day03;

[Puzzle(2021, 3, "Binary Diagnostic")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private int _bitCount;

    private List<int> _binaryNumbers = [];

    public void ParseInput(string[] inputLines)
    {
        _bitCount = inputLines[0].Length;
        _binaryNumbers = inputLines.Select(line => Convert.ToInt32(line, 2))
                                   .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var gamma = 0L;

        var bitMask = 1 << (_bitCount - 1);
        while (bitMask != 0)
        {
            var setBitCount = _binaryNumbers.Count(bn => (bn & bitMask) == bitMask);
            var notSetBitCount = _binaryNumbers.Count - setBitCount;

            if (setBitCount > notSetBitCount)
            {
                gamma = gamma * 2 + 1;
            }
            else
            {
                gamma *= 2;
            }

            bitMask >>= 1;
        }

        var epsilonBitMask = (1 << _bitCount) - 1;
        var epsilon = gamma ^ epsilonBitMask;

        var answer = gamma * epsilon;

        return new PuzzleAnswer(answer, 3309596);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var oxygenGeneratorRating = GetOxygenGeneratorRating();
        var co2ScrubberRating = GetCO2ScrubberRating();

        var answer = Convert.ToInt64(oxygenGeneratorRating) * co2ScrubberRating;

        return new PuzzleAnswer(answer, 2981085);
    }

    private int GetOxygenGeneratorRating()
    {
        var binaryNumbers = _binaryNumbers;

        var bitMask = 1 << (_bitCount - 1);
        while (bitMask != 0 && binaryNumbers.Count != 1)
        {
            var setBitCount = binaryNumbers.Count(bn => (bn & bitMask) == bitMask);
            var notSetBitCount = binaryNumbers.Count - setBitCount;

            binaryNumbers = notSetBitCount <= setBitCount
                ? binaryNumbers.Where(bn => (bn & bitMask) == bitMask).ToList()
                : binaryNumbers.Where(bn => (bn & bitMask) == 0).ToList();

            bitMask >>= 1;
        }

        return binaryNumbers[0];
    }

    private int GetCO2ScrubberRating()
    {
        var binaryNumbers = _binaryNumbers;

        var bitMask = 1 << (_bitCount - 1);
        while (bitMask != 0 && binaryNumbers.Count != 1)
        {
            var setBitCount = binaryNumbers.Count(bn => (bn & bitMask) == bitMask);
            var notSetBitCount = binaryNumbers.Count - setBitCount;

            binaryNumbers = notSetBitCount <= setBitCount
                ? binaryNumbers.Where(bn => (bn & bitMask) == 0).ToList()
                : binaryNumbers.Where(bn => (bn & bitMask) == bitMask).ToList();

            bitMask >>= 1;
        }

        return binaryNumbers[0];
    }
}