using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Utilities.Extensions;

internal static class CollectionExtensions
{
    public static List<string[]> SelectToChunks(this IEnumerable<string> source)
    {
        var chunksList = new List<string[]>();

        var items = new List<string>();

        foreach (var item in source)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                if (items.Count > 0)
                {
                    chunksList.Add([.. items]);
                }

                items = [];
            }
            else
            {
                items.Add(item);
            }
        }

        if (items.Count > 0)
        {
            chunksList.Add([.. items]);
        }

        return chunksList;
    }

    public static Grid<T> SelectToGrid<T>(this string[] lines, Func<char, T> func)
    {
        var grid = new Grid<T>(lines.Length, lines[0].Length);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = func(lines[row][column]);
            }
        }

        return grid;
    }
}
