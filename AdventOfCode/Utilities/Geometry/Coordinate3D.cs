using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Coordinate3D(long x, long y, long z)
{
    public long X { get; } = x;

    public long Y { get; } = y;

    public long Z { get; } = z;

    public static Coordinate3D Parse(string input)
    {
        var values = input.Split(',')
                          .Select(long.Parse)
                          .ToList();

        return new Coordinate3D(values[0], values[1], values[2]);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Coordinate3D otherCoordinate && X == otherCoordinate.X && Y == otherCoordinate.Y && Z == otherCoordinate.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }

    public IEnumerable<Coordinate3D> Sides()
    {
        var vectors = new Coordinate3D[]
        {
            new(1, 0, 0),
            new(-1, 0, 0),

            new(0, 1, 0),
            new(0, -1, 0),

            new(0, 0, 1),
            new(0, 0, -1),
        };

        foreach (var vector in vectors)
        {
            yield return this + vector;
        }
    }

    public static bool operator ==(Coordinate3D left, Coordinate3D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinate3D left, Coordinate3D right)
    {
        return !(left == right);
    }

    public static Coordinate3D operator +(Coordinate3D left, Coordinate3D right)
    {
        return new Coordinate3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }
}
