using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day13;

[Puzzle(2022, 13, "Distress Signal")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private List<ListPacket> _packets = [];

    public void ParseInput(string[] inputLines)
    {
        _packets = inputLines.Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(Packet.Parse)
                             .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var index = 0;
        var answer = 0;

        foreach (var chunk in _packets.Chunk(2))
        {
            index++;

            var result = chunk[0].CompareTo(chunk[1]);
            if (result < 0)
            {
                answer += index;
            }
        }

        return new PuzzleAnswer(answer, 5292);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var dividerPacket1 = Packet.Parse("[[2]]");
        var dividerPacket2 = Packet.Parse("[[6]]");

        var packets = new List<ListPacket>(_packets)
        {
            dividerPacket1,
            dividerPacket2
        };

        packets.Sort();

        var answer = (packets.IndexOf(dividerPacket1) + 1) * (packets.IndexOf(dividerPacket2) + 1);

        return new PuzzleAnswer(answer, 23868);
    }
}