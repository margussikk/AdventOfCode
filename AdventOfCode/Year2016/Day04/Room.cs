using AdventOfCode.Utilities.Extensions;
using System.Text;

namespace AdventOfCode.Year2016.Day04;
internal class Room
{
    public string Name { get; private set; } = string.Empty;
    public int SectorId { get; private set; }
    public string Checksum { get; private set; } = string.Empty;

    public bool IsReal()
    {
        var letterCounts = new Dictionary<char, int>();
        foreach (var c in Name.Where(c => c != '-'))
        {
            letterCounts.IncrementValue(c, 1);
        }

        var sortedLetters = letterCounts
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .Take(5)
            .Select(kv => kv.Key);

        var calculatedChecksum = new string([.. sortedLetters]);
        return calculatedChecksum == Checksum;
    }

    public string GetDecryptedName()
    {
        var stringBuilder = new StringBuilder();

        foreach (var c in Name)
        {
            if (c == '-')
            {
                stringBuilder.Append(' ');
            }
            else
            {
                var shifted = Convert.ToChar(((c - 'a' + SectorId) % ('z' - 'a' + 1)) + 'a');
                stringBuilder.Append(shifted);
            }
        }

        return stringBuilder.ToString();
    }

    public static Room Parse(string line)
    {
        var lastDashIndex = line.LastIndexOf('-');
        var bracketOpenIndex = line.IndexOf('[');
        var bracketCloseIndex = line.IndexOf(']');
        var name = line[0..lastDashIndex];
        var sectorId = int.Parse(line[(lastDashIndex + 1)..bracketOpenIndex]);
        var checksum = line[(bracketOpenIndex + 1)..bracketCloseIndex];

        return new Room
        {
            Name = name,
            SectorId = sectorId,
            Checksum = checksum
        };
    }
}
