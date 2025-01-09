using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2020.Day23;

[Puzzle(2020, 23, "Crab Cups")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private List<int> _cupNumbers = [];

    public void ParseInput(string[] inputLines)
    {
        _cupNumbers = inputLines[0].Select(c => c.ParseToDigit())
                                   .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var nextCupNumbers = GetAnswer(_cupNumbers.Count, 100);

        var answer = 0;

        var nextCup = nextCupNumbers[1];
        while (nextCup != 1)
        {
            answer = answer * 10 + nextCup;
            nextCup = nextCupNumbers[nextCup];
        }

        return new PuzzleAnswer(answer, 97624853);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var nextCupNumbers = GetAnswer(1_000_000, 10_000_000);

        var firstAfter = nextCupNumbers[1];
        var secondAfter = nextCupNumbers[firstAfter];

        var answer = Convert.ToInt64(firstAfter) * secondAfter;

        return new PuzzleAnswer(answer, 664642452305L);
    }

    private int[] GetAnswer(int cupsCount, int movesCount)
    {
        var nextCupNumbers = new int[cupsCount + 1];

        var previousCupNumber = 0;

        // The First numbers are from input
        foreach (var number in _cupNumbers)
        {
            nextCupNumbers[previousCupNumber] = number;
            previousCupNumber = number;
        }

        // Add more numbers for part 2 
        for (var number = _cupNumbers.Count + 1; number <= cupsCount; number++)
        {
            nextCupNumbers[previousCupNumber] = number;
            previousCupNumber = number;
        }

        nextCupNumbers[previousCupNumber] = nextCupNumbers[0];

        var currentCup = nextCupNumbers[0];
        for (var move = 0; move < movesCount; move++)
        {
            // Pick up cups
            var firstPickedUp = nextCupNumbers[currentCup];
            var secondPickedUp = nextCupNumbers[firstPickedUp];
            var thirdPickedUp = nextCupNumbers[secondPickedUp];

            // Jump over picked up cups
            nextCupNumbers[currentCup] = nextCupNumbers[thirdPickedUp];

            // Find the destination cup
            var destinationCup = currentCup == 1
                ? nextCupNumbers.Length - 1
                : currentCup - 1;

            while (destinationCup == firstPickedUp ||
                   destinationCup == secondPickedUp ||
                   destinationCup == thirdPickedUp)
            {
                destinationCup = destinationCup == 1
                    ? nextCupNumbers.Length - 1
                    : destinationCup - 1;
            }

            // Link picked up cups after the destination cup. Set tail first.
            nextCupNumbers[thirdPickedUp] = nextCupNumbers[destinationCup];
            nextCupNumbers[destinationCup] = firstPickedUp;

            currentCup = nextCupNumbers[currentCup];
        }

        return nextCupNumbers;
    }
}