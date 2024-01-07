using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Coordinate2D(long x, long y)
{
    public long X { get; } = x;

    public long Y { get; } = y;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Coordinate2D otherCoordinate && X == otherCoordinate.X && Y == otherCoordinate.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
