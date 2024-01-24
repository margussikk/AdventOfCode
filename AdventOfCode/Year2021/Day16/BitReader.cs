namespace AdventOfCode.Year2021.Day16;

internal class BitReader(byte[] bytes)
{
    private const int BITS = 8;
    private readonly byte[] _bytes = bytes;

    public int BitIndex { get; private set; } = 0;

    public int Read(int bitCount)
    {
        int value = 0;

        while (bitCount > 0)
        {
            value <<= 1;

            var bitMask = 1 << (BITS - 1 - (BitIndex % BITS));
            if ((_bytes[BitIndex / BITS] & bitMask) == bitMask)
            {
                value++;
            }

            BitIndex++;
            bitCount--;
        }

        return value;
    }
}
