using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Utilities.Extensions;

internal static class CollectionExtensions
{
    public static IEnumerable<List<string>> SelectToChunks(this IEnumerable<string> source)
    {
        var items = new List<string>();

        foreach (var item in source)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                yield return items;
                items = [];
            }
            else
            {
                items.Add(item);
            }
        }

        if (items.Count > 0)
        {
            yield return items;
        }
    }

    public static Grid<T> SelectToGrid<T>(this List<string> lines, Func<int, int, char, T> func)
    {
        var grid = new Grid<T>(lines.Count, lines[0].Length);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = func(row, column, lines[row][column]);
            }
        }

        return grid;
    }

    public static Grid<T> SelectToGrid<T>(this List<string> lines, Func<char, T> func)
    {
        var grid = new Grid<T>(lines.Count, lines[0].Length);

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
