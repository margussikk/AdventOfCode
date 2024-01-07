using System.Collections;
using System.Numerics;

namespace AdventOfCode.Utilities.Numerics;

internal readonly struct NumberRange<T>(T Start, T End) : IEnumerable<T> where T : IBinaryNumber<T>
{
    public T Length => End - Start + T.One;

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
}
