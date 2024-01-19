using System.Collections;
using System.Globalization;
using System.Numerics;

namespace AdventOfCode.Utilities.Numerics;
internal readonly struct NumberRange<T>(T start, T end) : IEnumerable<T> where T : IBinaryNumber<T>, IParsable<T>
{
    public T Start { get; } = start;

    public T End { get; } = end;

    public T Length => End - Start + T.One;

    public bool IsFullyContained(NumberRange<T> other)
    {
        return Start <= other.Start &&
               other.Start >= Start &&
               other.End <= End;
    }

    public bool IsOverlapped(NumberRange<T> other)
    {
        return Start <= other.Start &&
               other.Start <= End;
    }

    public NumberRange<T>[] SplitBefore(T splitBefore)
    {
        return
        [
            new NumberRange<T>(Start, splitBefore - T.One),
            new NumberRange<T>(splitBefore, End)
        ];
    }

    public NumberRange<T>[] SplitAfter(T splitAfter)
    {
        return
        [
            new NumberRange<T>(Start, splitAfter),
            new NumberRange<T>(splitAfter + T.One, End)
        ];
    }

    public bool Contains(T value)
    {
        return value >= Start && value <= End;
    }

    public override string ToString()
    {
        return $"[{Start}..{End}]";
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var value = Start; value <= End; value++)
        {
            yield return value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static NumberRange<T> Parse(string input)
    {
        var splits = input.Split('-');

        var start1 = T.Parse(splits[0], CultureInfo.InvariantCulture);
        var end1 = T.Parse(splits[1], CultureInfo.InvariantCulture);

        return new NumberRange<T>(start1, end1);
    }

    public static NumberRange<T>[] Merge(NumberRange<T>[] numberRanges)
    {
        if (numberRanges.Length <= 1)
        {
            return numberRanges;
        }

        var stack = new Stack<NumberRange<T>>();

        var sortedRanges = numberRanges.OrderBy(r => r.Start).ToArray();
        stack.Push(sortedRanges[0]);

        foreach (var currentRange in sortedRanges.Skip(1))
        {
            var lastRange = stack.Pop();

            if (lastRange.IsOverlapped(currentRange) || (lastRange.End + T.One == currentRange.Start))
            {
                var max = T.Max(lastRange.End, currentRange.End);
                var newLastRange = new NumberRange<T>(lastRange.Start, max);

                stack.Push(newLastRange);
            }
            else
            {
                stack.Push(lastRange);
                stack.Push(currentRange);
            }
        }

        return [.. stack.OrderBy(r => r.Start)];
    }
}
