namespace AdventOfCode.Utilities.Numerics;

internal static class BitFunctions
{
    public static uint ReverseBits(uint num, int bitCount)
    {
        uint reverse_num = 0;

        while (bitCount != 0)
        {
            reverse_num <<= 1;
            reverse_num |= num & 1;
            num >>= 1;
            bitCount--;
        }

        return reverse_num;
    }
}
