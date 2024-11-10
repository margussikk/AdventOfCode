using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2021.Day07;

[Puzzle(2021, 7, "The Treachery of Whales")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private List<int> _horizontalPositions = [];

    public void ParseInput(string[] inputLines)
    {
        _horizontalPositions = inputLines[0]
            .Split(',')
            .Select(int.Parse)
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(x => x);

        return new PuzzleAnswer(answer, 339321);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(FuelCalculator);

        return new PuzzleAnswer(answer, 95476244L);

        static int FuelCalculator(int x) => x * (x + 1) / 2;
    }

    private int GetAnswer(Func<int, int> calculateFuel)
    {
        var searchRange = new NumberRange<int>(0, _horizontalPositions.Max());

        while (searchRange.Length > 1)
        {
            var middle = (searchRange.Start + searchRange.End) / 2;
            var searchRanges = searchRange.SplitAfter(middle);

            var leftStartSum = _horizontalPositions.Sum(x => calculateFuel(Math.Abs(x - searchRanges[0].Start)));
            var leftEndSum = _horizontalPositions.Sum(x => calculateFuel(Math.Abs(x - searchRanges[0].End)));
            var leftSum = Math.Min(leftStartSum, leftEndSum);

            var rightStartSum = _horizontalPositions.Sum(x => calculateFuel(Math.Abs(x - searchRanges[1].Start)));
            var rightEndSum = _horizontalPositions.Sum(x => calculateFuel(Math.Abs(x - searchRanges[1].End)));
            var rightSum = Math.Min(rightStartSum, rightEndSum);

            if (leftSum < rightSum)
            {
                searchRange = searchRanges[0];
            }
            else if (rightSum < leftSum)
            {
                searchRange = searchRanges[1];
            }
            else
            {
                searchRange = new NumberRange<int>(middle, middle);
            }
        }

        return _horizontalPositions.Sum(x => calculateFuel(Math.Abs(x - searchRange.Start)));
    }

    private int GetAnswerOld(Func<int, int> calculateFuel)
    {
        var start = 0;
        var end = _horizontalPositions.Max();

        while (end - start > 1)
        {
            var pivot = Convert.ToInt32(Math.Round((start + end) / 2.0));

            var left = (start + pivot) / 2;
            var leftSum = _horizontalPositions.Sum(x => calculateFuel(x - left));

            var right = (pivot + end) / 2;
            var rightSum = _horizontalPositions.Sum(x => calculateFuel(x - right));

            if (leftSum < rightSum)
            {
                end = pivot;
            }
            else if (leftSum > rightSum)
            {
                start = pivot;
            }
            else
            {
                start = end = pivot;
            }
        }

        var sum1 = _horizontalPositions.Sum(x => calculateFuel(x - start));
        var sum2 = _horizontalPositions.Sum(x => calculateFuel(x - end));

        return Math.Min(sum1, sum2);
    }
}