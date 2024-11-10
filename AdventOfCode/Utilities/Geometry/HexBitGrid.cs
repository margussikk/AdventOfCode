using AdventOfCode.Utilities.Numerics;
using System.Collections;

namespace AdventOfCode.Utilities.Geometry;

internal class HexBitGrid : IEnumerable<HexGridCell<bool>>
{
    private readonly BitArray _bitArray;

    public NumberRange<int> QRange { get; }

    public NumberRange<int> RRange { get; }

    public HexBitGrid(NumberRange<int> qRange, NumberRange<int> rRange)
    {
        QRange = qRange;
        RRange = rRange;

        _bitArray = new BitArray(QRange.Length * RRange.Length);
    }

    public bool this[int q, int r]
    {
        get => _bitArray.Get(GetIndex(q, r));
        set => _bitArray.Set(GetIndex(q, r), value);
    }

    public bool this[HexCoordinate coordinate]
    {
        get => this[coordinate.Q, coordinate.R];
        set => this[coordinate.Q, coordinate.R] = value;
    }

    public bool InBounds(int q, int r)
    {
        return QRange.Contains(q) && RRange.Contains(r);
    }

    public bool InBounds(HexCoordinate coordinate)
    {
        return InBounds(coordinate.Q, coordinate.R);
    }

    public IEnumerable<HexGridCell<bool>> AroundNeighbors(HexCoordinate coordinate)
    {
        foreach (var neighborCoordinate in coordinate.AroundNeighbors().Where(InBounds))
        {
            yield return new HexGridCell<bool>(neighborCoordinate, this[neighborCoordinate]);
        }
    }

    public IEnumerable<HexGridCell<bool>> Window(NumberRange<int> qRange, NumberRange<int> rRange)
    {
        for (var q = qRange.Start; q <= qRange.End; q++)
        {
            var rStart = int.Max(rRange.Start, -q + rRange.Start);
            var rEnd = int.Min(rRange.End, -q + rRange.End);

            for (var r = rStart; r <= rEnd; r++)
            {
                yield return new HexGridCell<bool>(new HexCoordinate(q, r), this[q, r]);
            }
        }
    }

    public IEnumerator<HexGridCell<bool>> GetEnumerator()
    {
        return Window(QRange, RRange).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private int GetIndex(int q, int r)
    {
        return (q - QRange.Start) * RRange.Length + (r - RRange.Start);
    }
}
