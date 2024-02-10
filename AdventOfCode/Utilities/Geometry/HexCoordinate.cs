using BenchmarkDotNet.Columns;

namespace AdventOfCode.Utilities.Geometry;

// Q - West/East
// R - Northwest/Southeast
// S to Northeast/Southwest. 
// https://backdrifting.net/post/064_hex_grids

internal readonly struct HexCoordinate(int q, int r) : IEquatable<HexCoordinate>
{
    public readonly int Q { get; } = q;

    public readonly int R { get; } = r;

    public readonly int S => -Q - R;

    public IEnumerable<HexCoordinate> AroundNeighbors()
    {
        yield return new HexCoordinate(Q - 1, R);
        yield return new HexCoordinate(Q + 1, R);

        yield return new HexCoordinate(Q - 1, R + 1);
        yield return new HexCoordinate(Q + 1, R - 1);

        yield return new HexCoordinate(Q, R + 1);
        yield return new HexCoordinate(Q, R - 1);
    }


    public HexCoordinate Move(GridDirection direction)
    {
        return direction switch
        {
            GridDirection.Left => new HexCoordinate(Q - 1, R),
            GridDirection.Right => new HexCoordinate(Q + 1, R),

            GridDirection.UpLeft => new HexCoordinate(Q - 1, R + 1),
            GridDirection.DownRight => new HexCoordinate(Q + 1, R - 1),

            GridDirection.UpRight => new HexCoordinate(Q, R + 1),
            GridDirection.DownLeft => new HexCoordinate(Q, R - 1),

            GridDirection.None => this,
            _ => throw new InvalidOperationException("Unexpected direction")
        };
    }

    public static bool operator ==(HexCoordinate coordinate1, HexCoordinate coordinate2)
    {
        return coordinate1.Equals(coordinate2);
    }

    public static bool operator !=(HexCoordinate coordinate1, HexCoordinate coordinate2)
    {
        return !coordinate1.Equals(coordinate2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Q, R);
    }

    public bool Equals(HexCoordinate other)
    {
        return Q == other.Q && R == other.R;
    }

    public override bool Equals(object? obj)
    {
        return obj is HexCoordinate coordinate && Equals(coordinate);
    }

    public override string ToString()
    {
        return $"{Q}, {R}, {S}";
    }
}
