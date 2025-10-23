using AdventOfCode.Utilities.Collections;
using AdventOfCode.Utilities.GridSystem;
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

    public static Grid<T> SelectToGrid<T>(this string[] lines, Func<char, GridCoordinate, T> func)
    {
        var grid = new Grid<T>(lines.Length, lines[0].Length);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = func(lines[row][column], new GridCoordinate(row, column));
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

    private static T[] GeneratePermutation<T>(T[] array, int[] sequence)
    {
        var clone = (T[])array.Clone();

        for (var i = 0; i < clone.Length - 1; i++)
        {
            (clone[i], clone[i + sequence[i]]) = (clone[i + sequence[i]], clone[i]);
        }

        return clone;
    }

    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> enumerable, int combinationSize)
    {
        var array = enumerable as T[] ?? [.. enumerable];

        var indices = Enumerable.Range(0, combinationSize).ToArray();
        while (indices[0] <= array.Length - combinationSize)
        {
            yield return indices.Select(i => array[i]);

            indices[combinationSize - 1]++;
            for (var i = combinationSize - 1; i > 0; i--)
            {
                if (indices[i] >= array.Length - (combinationSize - 1 - i))
                {
                    indices[i - 1]++;
                    for (var j = i; j < combinationSize; j++)
                    {
                        indices[j] = indices[j - 1] + 1;
                    }
                }
            }
        }
    }

    private static int[] GenerateSequence(long number, int size, long[] factorials)
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

    public static int GetSequenceHashCode<T>(this IEnumerable<T> lst)
    {
        unchecked
        {
            int hash = 19;
            foreach (T item in lst)
            {
                hash = hash * 31 + (item != null! ? item.GetHashCode() : 1);
            }
            return hash;
        }
    }

    public static IEnumerable<ValuePair<T>> Pairs<T>(this IEnumerable<T> enumerable)
    {
        var array = enumerable as T[] ?? enumerable.ToArray();

        for (var index1 = 0; index1 < array.Length - 1; index1++)
        {
            for (var index2 = index1 + 1; index2 < array.Length; index2++)
            {
                yield return new ValuePair<T>(array[index1], array[index2]);
            }
        }
    }

    public static IEnumerable<IList<T>> SlidingWindow<T>(this IEnumerable<T> source, int windowSize)
    {
        var buffer = new T[windowSize];

        var count = 0;
        foreach(var item in source)
        {
            for (var i = 1; i < windowSize; i++)
            {
                buffer[i - 1] = buffer[i];
            }

            buffer[^1] = item;

            count++;
            if (count >= windowSize)
            {
                yield return [.. buffer];
            }
        }
    }

    public static IEnumerable<TAccumulate> Accumulate<TAccumulate, TSource>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
    {
        var accumulation = seed;

        foreach (var item in source)
        {
            accumulation = func(accumulation, item);
            yield return accumulation;
        }
    }

    public static string JoinToString(this IEnumerable<char> source)
    {
        return new string([.. source]);
    }
}
