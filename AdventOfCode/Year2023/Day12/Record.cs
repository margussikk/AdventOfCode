using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2023.Day12;

internal class Record
{
    public List<Spring> Springs { get; }
    public List<int> Groups { get; }

    private Record(List<Spring> springs, List<int> groups)
    {
        Springs = springs;
        Groups = groups;
    }

    public static Record Parse(string input)
    {
        var splits = input.Split(' ');

        var springs = splits[0].Select(ParseSpringCondition)
                               .ToList();

        var groups = splits[1].SplitToNumbers<int>(',').ToList();

        return new Record(springs, groups);
    }

    public Record Unfolded()
    {
        var springs1 = new List<Spring>(Springs);
        var groups1 = new List<int>(Groups);

        for (var i = 0; i < 4; i++)
        {
            springs1.Add(Spring.Unknown);
            springs1.AddRange(Springs);

            groups1.AddRange(Groups);
        }

        return new Record(springs1, groups1);
    }

    // V1 uses record-specific cache, which is much smaller than V2 and also faster.
    public long CountArrangementsV1()
    {
        var cache = new Dictionary<(int, int, int), long>();

        return Recurse(0, 0, 0);

        long Recurse(int springIndex, int groupIndex, int currentGroupLength)
        {
            var key = (springIndex, groupIndex, currentGroupLength);
            if (cache.TryGetValue(key, out var total))
            {
                return total;
            }

            // Check if reached the end of springs
            if (springIndex == Springs.Count)
            {
                if (groupIndex == Groups.Count && currentGroupLength == 0)
                {
                    // Reached the end groups and no group is open.
                    return 1;
                }

                if (groupIndex == Groups.Count - 1 && Groups[groupIndex] == currentGroupLength)
                {
                    // The Last group is open, but the current length is the same group size, so everything's ok.
                    return 1;
                }

                // The Last group is half-filled or there are more groups left.
                return 0;
            }

            // Operational
            if (Springs[springIndex] is Spring.Operational or Spring.Unknown)
            {
                if (currentGroupLength == 0)
                {
                    // Continue looking for the current group
                    total += Recurse(springIndex + 1, groupIndex, 0);
                }
                else if (groupIndex < Groups.Count && Groups[groupIndex] == currentGroupLength)
                {
                    // Reached the end of the current group. Start looking for a new group.
                    total += Recurse(springIndex + 1, groupIndex + 1, 0);
                }
            }

            // Damaged
            if (Springs[springIndex] is Spring.Damaged or Spring.Unknown)
            {
                // Continue processing current group.
                total += Recurse(springIndex + 1, groupIndex, currentGroupLength + 1);
            }

            cache[key] = total;

            return total;
        }
    }

    // V2 uses global cache and memoizes all call parameters
    public long CountArrangementsV2(Dictionary<string, long> cache)
    {
        if (Groups.Count == 0)
        {
            return Springs.TrueForAll(x => x is Spring.Operational or Spring.Unknown)
                ? 1 // Assume that all the leftover springs are operational
                : 0;
        }

        if (Springs.Count < Groups.Sum() + Groups.Count - 1)
        {
            return 0; // Not enough springs for all the groups
        }

        var cacheKey = ToString();
        if (cache.TryGetValue(cacheKey, out var total))
        {
            return total;
        }

        // Treat first spring as operational
        if (Springs[0] is Spring.Operational or Spring.Unknown)
        {
            var newRecord = new Record(Springs[1..], Groups);
            total += newRecord.CountArrangementsV2(cache);
        }

        // Treat first {groupSize} springs as damaged
        var groupSize = Groups[0];
        if (Springs[..groupSize].TrueForAll(x => x is Spring.Damaged or Spring.Unknown))
        {
            if (Springs.Count == groupSize)
            {
                var newRecord = new Record(Springs[groupSize..], Groups[1..]);
                total += newRecord.CountArrangementsV2(cache);
            }
            else if (Springs[groupSize] is Spring.Operational or Spring.Unknown)
            {
                var newRecord = new Record(Springs[(groupSize + 1)..], Groups[1..]);
                total += newRecord.CountArrangementsV2(cache);
            }
        }

        cache[cacheKey] = total;

        return total;
    }

    public override string ToString()
    {
        var text = Springs.Select(spring =>
        {
            return spring switch
            {
                Spring.Operational => '.',
                Spring.Damaged => '#',
                Spring.Unknown => '?',
                _ => throw new InvalidOperationException()
            };
        }).ToArray();

        return string.Join(string.Empty, text) + " " + string.Join(",", Groups);
    }

    private static Spring ParseSpringCondition(char letter)
    {
        return letter switch
        {
            '.' => Spring.Operational,
            '#' => Spring.Damaged,
            '?' => Spring.Unknown,
            _ => throw new InvalidOperationException()
        };
    }
}
