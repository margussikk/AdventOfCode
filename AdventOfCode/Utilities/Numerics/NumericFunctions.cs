namespace AdventOfCode.Utilities.Numerics;
internal static class NumericFunctions
{
    public static int DigitCount(this int number) => DigitCount(Convert.ToInt64(number));

    public static int DigitCount(this long number)
    {
        var  length = 1;

        if (number >= 10_000_000_000_000_000L)
        {
            length += 16;
            number /= 10_000_000_000_000_000L;
        }

        if (number >= 100_000_000L)
        {
            length += 8;
            number /= 100_000_000L;
        }

        if (number >= 10_000L)
        {
            length += 4;
            number /= 10_000L;
        }

        if (number >= 100L)
        {
            length += 2;
            number /= 100L;
        }

        if (number >= 10L)
        {
            length += 1;
        }

        return length;
    }
}
