using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2020.Day15;

[Puzzle(2020, 15, "Rambunctious Recitation")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private int[] _startingNumbers = [];

    public void ParseInput(string[] inputLines)
    {
        _startingNumbers = inputLines[0].SplitToNumbers<int>(',');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetSpokenNumber(2020);

        return new PuzzleAnswer(answer, 694);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetSpokenNumber(30_000_000);

        return new PuzzleAnswer(answer, 21768614);
    }

    private int GetSpokenNumber(int spokenNumberIndex)
    {
        var lastSeenNumberIndexes = new Dictionary<int, int>();

        var lastNumber = 0;
        for (var numberIndex = 0; numberIndex < spokenNumberIndex; numberIndex++)
        {
            if (numberIndex < _startingNumbers.Length)
            {
                lastNumber = _startingNumbers[numberIndex];

                if (numberIndex < _startingNumbers.Length - 1)
                {
                    lastSeenNumberIndexes[lastNumber] = numberIndex;
                }
            }
            else if (lastSeenNumberIndexes.TryGetValue(lastNumber, out var lastSeenNumberIndex))
            {
                lastSeenNumberIndexes[lastNumber] = numberIndex - 1;
                lastNumber = numberIndex - 1 - lastSeenNumberIndex;
            }
            else
            {
                lastSeenNumberIndexes[lastNumber] = numberIndex - 1;
                lastNumber = 0;
            }
        }

        return lastNumber;
    }
}