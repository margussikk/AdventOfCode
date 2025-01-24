using System.Collections;
using System.Numerics;

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

    public static int PopCount(this BitArray bitArray)
    {
        var count = 0;

        uint[] ints = new uint[(bitArray.Count >> 5) + 1];
        bitArray.CopyTo(ints, 0);

        for (var i = 0; i < ints.Length; i++)
        {
            count += BitOperations.PopCount(ints[i]);
        }

        return count;
    }
}
