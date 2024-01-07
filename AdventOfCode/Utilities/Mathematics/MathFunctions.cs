using System.Numerics;

namespace AdventOfCode.Utilities.Mathematics;

internal static class MathFunctions
{
    public static T Modulo<T>(T first, T second) where T : INumber<T>
    {
        var c = first % second;
        return (c < T.Zero) ? c + second : c;
    }

    public static T GreatestCommonDivisor<T>(T first, T second) where T : INumber<T>
    {
        while (second != T.Zero)
        {
            var temp = second;
            second = first % second;
            first = temp;
        }

        return first;
    }

    public static T LeastCommonMultiple<T>(T first, T second) where T : INumber<T>
        => first / GreatestCommonDivisor(first, second) * second;

    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);
}
