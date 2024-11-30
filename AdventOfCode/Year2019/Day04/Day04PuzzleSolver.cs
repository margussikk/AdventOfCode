using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2019.Day04;

[Puzzle(2019, 4, "Secure Container")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private NumberRange<int> _passwordRange = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _passwordRange = NumberRange<int>.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CountPasswords(0, 100_000, AnyAdjacentDigitsCheck);

        return new PuzzleAnswer(answer, 1873);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = CountPasswords(0, 100_000, OnlyTwoAdjacentDigitsCheck);

        return new PuzzleAnswer(answer, 1264);
    }

    private int CountPasswords(int baseValue, int divider, Func<int, bool> adjacentDigitsCheck)
    {
        var count = 0;

        var minBaseValue = _passwordRange.Start / divider * divider;
        var maxBaseValue = _passwordRange.End / divider * divider;

        var minDigit = baseValue / (divider * 10) % 10;
        for (var digit = minDigit; digit <= 9; digit++)
        {
            var newBaseValue = baseValue + digit * divider;
            if (newBaseValue < minBaseValue || newBaseValue > maxBaseValue) continue;

            if (divider == 1)
            {
                if (adjacentDigitsCheck(newBaseValue))
                {
                    count++;
                }
            }
            else
            {
                count += CountPasswords(newBaseValue, divider / 10, adjacentDigitsCheck);
            }
        }

        return count;
    }

    private static bool AnyAdjacentDigitsCheck(int number)
    {
        int? previousLast = null;

        while (number > 0)
        {
            var currentLast = number % 10;
            if (currentLast == previousLast)
            {
                return true;
            }

            previousLast = currentLast;
            number /= 10;
        }

        return false;
    }

    private static bool OnlyTwoAdjacentDigitsCheck(int number)
    {
        var adjacencyCount = 0;
        int? previousLast = null;

        while (number > 0)
        {
            var currentLast = number % 10;
            if (currentLast == previousLast)
            {
                adjacencyCount++;
            }
            else if (adjacencyCount == 2)
            {
                return true;
            }
            else
            {
                adjacencyCount = 1;
            }

            previousLast = currentLast;
            number /= 10;
        }

        return adjacencyCount == 2;
    }
}