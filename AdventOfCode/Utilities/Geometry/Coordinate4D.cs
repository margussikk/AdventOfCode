using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Coordinate4D(long x, long y, long z, long w) : IEquatable<Coordinate4D>
{
    public long X { get; } = x;

    public long Y { get; } = y;

    public long Z { get; } = z;

    public long W { get; } = w;

    public static Coordinate4D Parse(string input)
    {
        var values = input.Split(',')
                          .Select(long.Parse)
                          .ToList();

        return new Coordinate4D(values[0], values[1], values[2], values[3]);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }

    public override string ToString()
    {
        return $"{X},{Y},{Z},{W}";
    }
    
    public IEnumerable<Coordinate4D> AroundNeighbors()
    {
        for (var dw = -1; dw <= 1; dw++)
        {
            for (var dz = -1; dz <= 1; dz++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    for (var dx = -1; dx <= 1; dx++)
                    {
                        if (dx != 0 || dy != 0 || dz != 0 || dw != 0)
                        {
                            yield return new Coordinate4D(X + dx, Y + dy, Z + dz, W + dw);
                        }
                    }
                }
            }
        }
    }

    public static bool operator ==(Coordinate4D left, Coordinate4D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinate4D left, Coordinate4D right)
    {
        return !(left == right);
    }

    public bool Equals(Coordinate4D other)
    {
        return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Coordinate4D otherCoordinate &&
            X == otherCoordinate.X &&
            Y == otherCoordinate.Y &&
            Z == otherCoordinate.Z &&
            W == otherCoordinate.W;
    }
}
