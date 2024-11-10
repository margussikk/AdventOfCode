namespace AdventOfCode.Utilities.Extensions;

internal static class StringExtensions
{
    public static IReadOnlyList<long> SelectToLongs(this string source, params char[] separators)
    {
        var sign = 1;
        var items = new List<long>();
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
                default:
                {
                    if (separators.Contains(span[0]))
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

                    break;
                }
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
