using System.Numerics;
using System.Runtime.InteropServices;

namespace AdventOfCode.Utilities.Extensions;

internal static class DictionaryExtensions
{
    public static void IncrementValue<T, TIncrement>(this Dictionary<T, TIncrement> dictionary, T key, TIncrement increment) where T : notnull
                                                                                                                             where TIncrement : struct, IBinaryNumber<TIncrement>
    {
        ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _);
        value += increment;
    }

    public static void AddToValueList<T, TItem>(this Dictionary<T, List<TItem>> dictionary, T key, TItem item) where T : notnull
    {
        ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _);
        value ??= [];
        value.Add(item);
    }
}
