using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2023.Day05;

[Puzzle(2023, 5, "If You Give A Seed A Fertilizer")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private List<long> _seeds = [];
    private List<Map> _maps = [];

    public void ParseInput(string[] inputLines)
    {
        _seeds = [.. inputLines[0]["seeds: ".Length..].SplitToNumbers<long>(' ')];

        _maps = inputLines
            .Skip(2)
            .SelectToChunks()
            .Select(chunk => chunk
                .Skip(1) // Skip name
                .Select(Mapping.Parse)
                .ToList())
            .Select(mappings => new Map(mappings))
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _seeds.Min(seed => _maps.Aggregate(seed, (sourceNumber, map) => map.GetDestinationNumber(sourceNumber)));

        return new PuzzleAnswer(answer, 318728750);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var seedNumberRanges = _seeds
            .Chunk(2)
            .Select(chunk => new NumberRange<long>(chunk[0], chunk[0] + chunk[1] - 1))
            .ToList();

        var answer = _maps.Aggregate(seedNumberRanges, MapToDestinationNumberRanges)
                          .Min(numberRange => numberRange.Start);

        return new PuzzleAnswer(answer, 37384986);
    }

    // Split ranges to multiple ranges
    private static List<NumberRange<long>> MapToDestinationNumberRanges(List<NumberRange<long>> numberRanges, Map map)
    {
        var destinationNumberRanges = new List<NumberRange<long>>();

        foreach (var initialNumberRange in numberRanges)
        {
            var currentNumberRange = initialNumberRange;

            while (currentNumberRange.Length > 0)
            {
                long mappingSourceRangeEnd;
                long destinationNumberRangeStart;

                var mapping = map.Mappings.Find(m => m.SourceRange.Contains(currentNumberRange.Start));
                if (mapping != null)
                {
                    // Mapping found. Range can't exceed sources end. Also find destination number from the mapping.

                    mappingSourceRangeEnd = mapping.SourceRange.End;
                    destinationNumberRangeStart = mapping.GetDestination(currentNumberRange.Start);
                }
                else
                {
                    // Find the first mapping that could end the current range
                    mappingSourceRangeEnd = map.Mappings
                        .Where(m => currentNumberRange.Start < m.SourceRange.Start)
                        .Select(m => m.SourceRange.Start)
                        .DefaultIfEmpty(long.MaxValue)
                        .Min();

                    // Since mapping was not found, destination number is the same as source number
                    destinationNumberRangeStart = currentNumberRange.Start;
                }

                // Can't exceed mapping end or current ranges end
                var rangeEnd = long.Min(currentNumberRange.End, mappingSourceRangeEnd);

                // Store the left side of the split
                var destinationNumberRange = new NumberRange<long>(destinationNumberRangeStart, destinationNumberRangeStart + rangeEnd - currentNumberRange.Start);
                destinationNumberRanges.Add(destinationNumberRange);

                if (currentNumberRange.End == rangeEnd) // Reached the end of the range
                {
                    break;
                }

                // Continue with the right side of the split
                currentNumberRange = new NumberRange<long>(rangeEnd + 1, currentNumberRange.End);
            }
        }

        return destinationNumberRanges;
    }
}
