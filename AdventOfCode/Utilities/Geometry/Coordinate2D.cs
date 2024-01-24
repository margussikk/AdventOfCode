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

    public static Vector2D operator -(Coordinate2D coordinate1, Coordinate2D coordinate2)
    {
        return new Vector2D(coordinate1.X - coordinate2.X, coordinate1.Y - coordinate2.Y);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Coordinate2D otherCoordinate && X == otherCoordinate.X && Y == otherCoordinate.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static Coordinate2D Parse(string input)
    {
        var splits = input.Split(',');

        return new Coordinate2D(long.Parse(splits[0]), long.Parse(splits[1]));
    }
}
