using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2023.Day05;

internal class Mapping
{
    public NumberRange<long> SourceRange { get; private set; }

    public long DestinationRangeStart { get; private set; }

    public long GetDestination(long sourceNumber) => DestinationRangeStart + sourceNumber - SourceRange.Start;

    public static Mapping Parse(string input)
    {
        var values = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                          .Select(long.Parse)
                          .ToList();

        var mapping = new Mapping
        {
            DestinationRangeStart = values[0],
            SourceRange = new NumberRange<long>(values[1], values[1] + values[2] - 1)
        };

        return mapping;
    }
}
