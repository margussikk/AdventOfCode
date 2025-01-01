using AdventOfCode.Utilities.Geometry;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day03;

internal partial class Elf
{
    public int Id { get; init; }

    public required Region2D ClaimArea { get; init; }

    public bool TryFindOverlappingClaimArea(Elf other, [MaybeNullWhen(false)] out Region2D overlapArea)
    {
        return ClaimArea.TryFindOverlap(other.ClaimArea, out overlapArea);
    }

    public static Elf Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var minCoordinate = Coordinate2D.Parse(match.Groups[2].Value);
        var sizeVector = Vector2D.Parse(match.Groups[3].Value.Replace('x', ','));
        var maxCoordinate = minCoordinate + sizeVector - Vector2D.UnitX - Vector2D.UnitY;

        return new Elf
        {
            Id = int.Parse(match.Groups[1].Value),
            ClaimArea = new Region2D(minCoordinate, maxCoordinate)
        };
    }

    [GeneratedRegex(@"#(\d+) @ (\d+,\d+): (\d+x\d+)")]
    private static partial Regex InputLineRegex();
}
