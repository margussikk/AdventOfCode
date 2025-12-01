using AdventOfCode.Utilities.Mathematics;
using System.Globalization;

namespace AdventOfCode.Year2017.Common;

internal static class Hash
{
    public static byte[] SparseHash(byte[] lengths, int rounds)
    {
        var elements = Enumerable.Range(0, 256)
                                 .Select(Convert.ToByte)
                                 .ToArray();

        var skipSize = 0;
        var currentPosition = 0;

        for (var round = 0; round < rounds; round++)
        {
            foreach (var length in lengths)
            {
                var position1 = currentPosition;
                var position2 = (currentPosition + (length - 1)) % elements.Length;

                for (var i = 0; i < length / 2; i++)
                {
                    (elements[position1], elements[position2]) = (elements[position2], elements[position1]);

                    position1 = MathFunctions.Modulo(position1 + 1, elements.Length);
                    position2 = MathFunctions.Modulo(position2 - 1, elements.Length);
                }

                currentPosition = (currentPosition + length + skipSize) % elements.Length;
                skipSize++;
            }
        }

        return elements;
    }

    public static string KnotHash(string input)
    {
        var lengths = input.Select(Convert.ToByte)
                    .Concat(new byte[] { 17, 31, 73, 47, 23 })
                    .ToArray();

        var sparseHash = SparseHash(lengths, 64);

        var denseHash = sparseHash
            .Chunk(16)
            .Select(chunk => chunk.Aggregate((byte)0, (agg, current) => (byte)(agg ^ current)))
            .ToArray();

        return Convert.ToHexStringLower(denseHash);
    }
}
