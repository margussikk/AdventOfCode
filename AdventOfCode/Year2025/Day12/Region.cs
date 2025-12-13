using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2025.Day12;

internal class Region
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int[] Quantities { get; set; } = [];

    public static Region Parse(string line)
    {
        var parts = line.Split(':', StringSplitOptions.TrimEntries);

        var dimensions = parts[0].SplitToNumbers<int>('x');

        return new Region
        {
            Width = dimensions[0],
            Height = dimensions[1],
            Quantities = parts[1].SplitToNumbers<int>(' ')
        };
    }
}
