using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Mathematics;
using System.Globalization;

namespace AdventOfCode.Year2017.Day10;

[Puzzle(2017, 10, "Knot Hash")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;


    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var lengths = _input.SplitToNumbers<byte>(',');
        var sparseHash = GetSparseHash(lengths, 1);

        var answer = sparseHash[0] * sparseHash[1];

        return new PuzzleAnswer(answer, 23874);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var lengths = _input.Select(Convert.ToByte)
                            .Concat(new byte[] { 17, 31, 73, 47, 23 })
                            .ToArray();

        var sparseHash = GetSparseHash(lengths, 64);

        var denseHash = sparseHash
            .Chunk(16)
            .Select(chunk => chunk.Aggregate((byte)0, (agg, current) => (byte)(agg ^ current)))
            .ToArray();

        var knotHash = Convert.ToHexString(denseHash)
                              .ToLower(CultureInfo.InvariantCulture);

        return new PuzzleAnswer(knotHash, "e1a65bfb5a5ce396025fab5528c25a87");
    }

    private static byte[] GetSparseHash(byte[] lengths, int rounds)
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
}