using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Collections;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2022.Day20;

[Puzzle(2022, 20, "Grove Positioning System")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private List<Number> _numbers = [];

    public void ParseInput(string[] inputLines)
    {
        _numbers = inputLines.Select(Number.Parse)
                            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var numbers = new List<Number>(_numbers);

        var answer = Mix(_numbers, numbers, 1);

        return new PuzzleAnswer(answer, 10831);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var inputNumbers = new List<Number>(_numbers.Select(x => new Number
        {
            Value = x.Value * 811589153L
        }));

        var numbers = new List<Number>(inputNumbers);

        var answer = Mix(inputNumbers, numbers, 10);

        return new PuzzleAnswer(answer, 6420481789383L);
    }

    private static long Mix(List<Number> inputNumbers, List<Number> numbers, int times)
    {
        var bucketList = new BucketList<Number>(numbers);

        for (var i = 0; i < times; i++)
        {
            foreach (var number in inputNumbers)
            {
                var currentIndex = bucketList.IndexOf(number);

                bucketList.RemoveAt(currentIndex);

                // Here list is 1 element shorter. First and the last element are
                // essentially at the same location in the circular list.
                // Using list.Count - 1, we can skip the last index and use index 0 instead.
                var newIndex = MathFunctions.Modulo(currentIndex + number.Value, bucketList.Count);
                bucketList.Insert(Convert.ToInt32(newIndex), number);
            }
        }

        var zero = bucketList.FindIndex(x => x.Value == 0);

        var thousandth = bucketList[(zero + 1000) % bucketList.Count];
        var twoThousandth = bucketList[(zero + 2000) % bucketList.Count];
        var threeThousandth = bucketList[(zero + 3000) % bucketList.Count];

        return thousandth.Value + twoThousandth.Value + threeThousandth.Value;
    }
}