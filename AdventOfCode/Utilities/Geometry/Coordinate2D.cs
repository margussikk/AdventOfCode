using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Coordinate2D(long x, long y)
{
    public long X { get; } = x;

    public long Y { get; } = y;

    public static Coordinate2D operator +(Coordinate2D coordinate, Vector2D vector)
    {
        return new Coordinate2D(coordinate.X + vector.DX, coordinate.Y + vector.DY);
    }

    public static Coordinate2D operator -(Coordinate2D coordinate, Vector2D vector)
    {
        return new Coordinate2D(coordinate.X - vector.DX, coordinate.Y - vector.DY);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Coordinate2D otherCoordinate && X == otherCoordinate.X && Y == otherCoordinate.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
