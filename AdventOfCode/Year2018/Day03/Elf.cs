using AdventOfCode.Utilities.Geometry;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day03
{
    internal partial class Elf
    {
        public int Id { get; init; }

        public required Area2D ClaimArea { get; init; }

        public bool TryFindOverlappingClaimArea(Elf other, [MaybeNullWhen(false)] out Area2D overlapArea)
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

            var minCoordinate = new Coordinate2D(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
            var maxCoordinate = new Coordinate2D(minCoordinate.X + int.Parse(match.Groups[4].Value) - 1, minCoordinate.Y + int.Parse(match.Groups[5].Value) - 1);

            return new Elf
            {
                Id = int.Parse(match.Groups[1].Value),
                ClaimArea = new Area2D(minCoordinate, maxCoordinate)
            };
        }

        [GeneratedRegex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)")]
        private static partial Regex InputLineRegex();
    }
}
