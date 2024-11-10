using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day16;

[Puzzle(2021, 16, "Packet Decoder")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private byte[] _bytes = [];

    public void ParseInput(string[] inputLines)
    {
        _bytes = new byte[inputLines[0].Length / 2 + inputLines[0].Length % 2];

        for (var i = 0; i < inputLines[0].Length; i++)
        {
            if (i % 2 == 0)
            {
                _bytes[i / 2] |= Convert.ToByte(inputLines[0][i] + "0", 16);
            }
            else
            {
                _bytes[i / 2] |= Convert.ToByte("0" + inputLines[0][i], 16);
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var bitReader = new BitReader(_bytes);
        var packet = Packet.Build(bitReader);

        var answer = packet.SumVersions();

        return new PuzzleAnswer(answer, 852);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var bitReader = new BitReader(_bytes);
        var packet = Packet.Build(bitReader);

        var answer = packet.Evaluate();

        return new PuzzleAnswer(answer, 19348959966392L);
    }
}