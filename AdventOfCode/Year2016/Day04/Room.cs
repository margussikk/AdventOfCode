using AdventOfCode.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day04;

internal partial class Room
{
    public string Name { get; private set; } = string.Empty;
    public int SectorId { get; private set; }
    public string Checksum { get; private set; } = string.Empty;

    public bool IsReal()
    {
        var calculatedChecksum = Name
            .Where(c => c != '-')
            .GroupBy(c => c)
            .OrderByDescending(kv => kv.Count())
            .ThenBy(kv => kv.Key)
            .Select(kv => kv.Key)
            .Take(5)
            .JoinToString();

        return calculatedChecksum == Checksum;
    }

    public string GetDecryptedName()
    {
        return Name.Select(c => c == '-' ? ' ' : Convert.ToChar((c - 'a' + SectorId) % ('z' - 'a' + 1) + 'a'))
                   .JoinToString();
    }

    public static Room Parse(string line)
    {
        var matches = InputLineRegex().Matches(line);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var name = match.Groups["name"].Value;
        var sectorId = int.Parse(match.Groups["sectorId"].Value);
        var checksum = match.Groups["checksum"].Value;

        return new Room
        {
            Name = name,
            SectorId = sectorId,
            Checksum = checksum
        };
    }

    [GeneratedRegex(@"(?<name>[a-z\-]+)\-(?<sectorId>\d+)\[(?<checksum>[a-z]+)\]", RegexOptions.ExplicitCapture)]
    private static partial Regex InputLineRegex();
}
