﻿using AdventOfCode.Utilities.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Coordinate3D(long x, long y, long z) : IEquatable<Coordinate3D>
{
    public static readonly Coordinate3D Zero = new(0, 0, 0);

    public long X { get; } = x;

    public long Y { get; } = y;

    public long Z { get; } = z;

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }

    public IEnumerable<Coordinate3D> SideNeighbors()
    {
        var vectors = new Vector3D[]
        {
            new(1, 0, 0),
            new(-1, 0, 0),

            new(0, 1, 0),
            new(0, -1, 0),

            new(0, 0, 1),
            new(0, 0, -1)
        };

        foreach (var vector in vectors)
        {
            yield return this + vector;
        }
    }

    public IEnumerable<Coordinate3D> AroundNeighbors()
    {
        for (var dz = -1; dz <= 1; dz++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx != 0 || dy != 0 || dz != 0)
                    {
                        yield return new Coordinate3D(X + dx, Y + dy, Z + dz);
                    }
                }
            }
        }
    }

    public long ManhattanDistanceTo(Coordinate3D other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
    }

    public List<Coordinate3D> Orientations()
    {
        // Guide https://www.euclideanspace.com/maths/algebra/matrix/transforms/examples/index.htm
        //             | 0   0   1|
        // |X, Y, Z| * | 0   1   0| = |-Z, Y, X|
        //             |-1   0   0|

        return
        [
            new Coordinate3D(X, Y, Z),
            new Coordinate3D(-Z, Y, X),
            new Coordinate3D(-X, Y, -Z),
            new Coordinate3D(Z, Y, -X),
            new Coordinate3D(Y, -X, Z),
            new Coordinate3D(Y, Z, X),
            new Coordinate3D(Y, X, -Z),
            new Coordinate3D(Y, -Z, -X),
            new Coordinate3D(-Y, X, Z),
            new Coordinate3D(-Y, -Z, X),
            new Coordinate3D(-Y, -X, -Z),
            new Coordinate3D(-Y, Z, -X),
            new Coordinate3D(X, Z, -Y),
            new Coordinate3D(-Z, X, -Y),
            new Coordinate3D(-X, -Z, -Y),
            new Coordinate3D(Z, -X, -Y),
            new Coordinate3D(X, -Y, -Z),
            new Coordinate3D(-Z, -Y, -X),
            new Coordinate3D(-X, -Y, Z),
            new Coordinate3D(Z, -Y, X),
            new Coordinate3D(X, -Z, Y),
            new Coordinate3D(-Z, -X, Y),
            new Coordinate3D(-X, Z, Y),
            new Coordinate3D(Z, X, Y)
        ];
    }

    public static bool operator ==(Coordinate3D left, Coordinate3D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinate3D left, Coordinate3D right)
    {
        return !(left == right);
    }

    public static Coordinate3D operator +(Coordinate3D coordinate, Vector3D vector)
    {
        return new Coordinate3D(coordinate.X + vector.DX, coordinate.Y + vector.DY, coordinate.Z + vector.DZ);
    }

    public static Coordinate3D operator -(Coordinate3D coordinate, Vector3D vector)
    {
        return new Coordinate3D(coordinate.X - vector.DX, coordinate.Y - vector.DY, coordinate.Z - vector.DZ);
    }

    public static Vector3D operator -(Coordinate3D coordinate1, Coordinate3D coordinate2)
    {
        return new Vector3D(coordinate1.X - coordinate2.X, coordinate1.Y - coordinate2.Y, coordinate1.Z - coordinate2.Z);
    }

    public bool Equals(Coordinate3D other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Coordinate3D otherCoordinate && X == otherCoordinate.X && Y == otherCoordinate.Y && Z == otherCoordinate.Z;
    }

    public static Coordinate3D Parse(string input)
    {
        var values = input.SplitToNumbers<long>(',', ' ');

        return new Coordinate3D(values[0], values[1], values[2]);
    }
}
