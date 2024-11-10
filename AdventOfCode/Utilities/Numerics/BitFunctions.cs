namespace AdventOfCode.Utilities.Numerics;

internal static class BitFunctions
{
    public static uint ReverseBits(uint num, int bitCount)
    {
        uint reverseNum = 0;

        while (bitCount != 0)
        {
            reverseNum <<= 1;
            reverseNum |= num & 1;
            num >>= 1;
            bitCount--;
        }

        return reverseNum;
    }
}
