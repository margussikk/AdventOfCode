using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Mathematics;

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

    public static BitGrid SelectToBitGrid(this string[] lines, Func<char, bool> func)
    {
        var grid = new BitGrid(lines.Length, lines[0].Length);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = func(lines[row][column]);
            }
        }

        return grid;
    }

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
    {
        var array = enumerable as T[] ?? enumerable.ToArray();

        var factorials = Enumerable.Range(0, array.Length + 1)
            .Select(MathFunctions.Factorial)
            .ToArray();

        for (var i = 0L; i < factorials[array.Length]; i++)
        {
            var sequence = GenerateSequence(i, array.Length - 1, factorials);

            yield return GeneratePermutation(array, sequence);
        }
    }

    private static T[] GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
    {
        var clone = (T[])array.Clone();

        for (int i = 0; i < clone.Length - 1; i++)
        {
            (clone[i], clone[i + sequence[i]]) = (clone[i + sequence[i]], clone[i]);
        }

        return clone;
    }

    private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
    {
        var sequence = new int[size];

        for (var j = 0; j < sequence.Length; j++)
        {
            var facto = factorials[sequence.Length - j];

            sequence[j] = (int)(number / facto);
            number = (int)(number % facto);
        }

        return sequence;
    }
}
