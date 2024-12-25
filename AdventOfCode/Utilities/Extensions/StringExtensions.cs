using System.Numerics;

namespace AdventOfCode.Utilities.Extensions;

internal static class StringExtensions
{
    public static T[] SplitToNumbers<T>(this string source, params char[] separators) where T : struct, IBinaryNumber<T>
    {
        var items = new List<T>();

        int? startIndex = null;

        for (var index = 0; index < source.Length; index++)
        {
            var character = source[index];

            switch (character)
            {
                case '-' when startIndex.HasValue:
                    throw new InvalidOperationException("- in the middle of the number");
                case '-' or >= '0' and <= '9':
                    if (!startIndex.HasValue)
                    {
                        startIndex = index;
                    }
                    break;
                default:
                    if (separators.Contains(character))
                    {
                        if (startIndex.HasValue)
                        {
                            items.Add(T.Parse(source[startIndex.Value..index].AsSpan(), provider: null));
                            startIndex = null;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unexcpected character '{character}'");
                    }

                    break;
            }
        }

        if (startIndex.HasValue)
        {
            items.Add(T.Parse(source[startIndex.Value..source.Length].AsSpan(), provider: null));
        }

        return [.. items];
    }

    public static long SelectToOneLong(this string source)
    {
        var sign = 1;
        long? value = null;

        var span = source.AsSpan();

        while (span.Length > 0)
        {
            switch (span[0])
            {
                case '-' when value.HasValue:
                    throw new InvalidOperationException("- in the middle of the number");
                case '-':
                    sign = -1;
                    break;
                case >= '0' and <= '9':
                    {
                        value ??= 0;

                        value = value * 10 + sign * (span[0] - '0');
                        break;
                    }
            }

            span = span[1..];
        }

        return value ?? 0;
    }
}
