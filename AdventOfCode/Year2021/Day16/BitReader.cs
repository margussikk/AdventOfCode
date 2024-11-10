namespace AdventOfCode.Year2021.Day16;

internal class BitReader
{
    private const int Bits = 8;
    private readonly byte[] _bytes;

    public int BitIndex { get; private set; }

    public BitReader(byte[] bytes)
    {
        _bytes = bytes;
    }

    public int Read(int bitCount)
    {
        var value = 0;

        while (bitCount > 0)
        {
            value <<= 1;

            var bitMask = 1 << (Bits - 1 - BitIndex % Bits);
            if ((_bytes[BitIndex / Bits] & bitMask) == bitMask)
            {
                value++;
            }

            BitIndex++;
            bitCount--;
        }

        return value;
    }
}
