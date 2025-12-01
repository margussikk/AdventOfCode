using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2024.Day07;

internal class Equation
{
    public long TestValue { get; private init; }

    public int[] Numbers { get; private init; } = [];

    public bool CouldProduce(bool concatenation)
        => Check(TestValue, Numbers[0], Numbers.AsSpan()[1..], concatenation);

    private static bool Check(long testValue, long currentValue, Span<int> numbers, bool concatenation)
    {
        if (currentValue > testValue)
        {
            return false;
        }

        if (numbers.Length == 0)
        {
            return testValue == currentValue;
        }

        return Check(testValue, currentValue + numbers[0], numbers[1..], concatenation) ||
               Check(testValue, currentValue * numbers[0], numbers[1..], concatenation) ||
               (concatenation && Check(testValue, currentValue * MathFunctions.Power10(numbers[0].DigitCount()) + numbers[0], numbers[1..], concatenation));
    }

    public static Equation Parse(string input)
    {
        var splits = input.Split(':');

        return new Equation
        {
            TestValue = long.Parse(splits[0]),
            Numbers = splits[1].SplitToNumbers<int>(' ')
        };
    }
}
