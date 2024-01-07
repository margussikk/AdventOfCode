namespace AdventOfCode.Utilities.Extensions;

internal static class StringExtensions
{
    public static IReadOnlyList<long> SelectToLongs(this string source, params char[] separators)
    {
        var sign = 1;
        var items = new List<long>();
        long? value = null;

        var span = source.AsSpan();

        while(span.Length > 0)
        {
            if (span[0] == '-')
            {
                if (value.HasValue)
                {
                    throw new InvalidOperationException("- in the middle of the number");
                }
                sign = -1;
            }
            else if (span[0] is >= '0' and <= '9')
            {
                if (!value.HasValue)
                {
                    value = 0;
                }

                value = value * 10 + sign * (span[0] - '0');
            }
            else if (separators.Contains(span[0]))
            {
                if (value.HasValue)
                {
                    items.Add(value.Value);
                    value = null;
                    sign = 1;
                }
            }
            else
            {
                throw new InvalidOperationException($"Unexcpected characted '{span[0]}'");
            }

            span = span[1..];
        }

        if (value.HasValue)
        {
            items.Add(value.Value);
        }

        return items;
    }

    public static long SelectToOneLong(this string source)
    {
        var sign = 1;
        long? value = null;

        var span = source.AsSpan();

        while (span.Length > 0)
        {
            if (span[0] == '-')
            {
                if (value.HasValue)
                {
                    throw new InvalidOperationException("- in the middle of the number");
                }
                sign = -1;
            }
            else if (span[0] is >= '0' and <= '9')
            {
                if (!value.HasValue)
                {
                    value = 0;
                }

                value = value * 10 + sign * (span[0] - '0');
            }

            span = span[1..];
        }

        if (value.HasValue)
        {
            return value.Value;
        }

        return 0;
    }
}
