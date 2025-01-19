using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Vector3D(long dX, long dY, long dZ) : IEquatable<Vector3D>
{
    public static readonly Vector3D UnitX = new(1, 0, 0);

    public static readonly Vector3D UnitY = new(0, 1, 0);

    public static readonly Vector3D UnitZ = new(0, 0, 1);

    public long DX { get; } = dX;

    public long DY { get; } = dY;

    public long DZ { get; } = dZ;

    public bool Equals(Vector3D other)
    {
        return DX == other.DX && DY == other.DY && DZ == other.DZ;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector3D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(DX, DY, DZ);
    }

    public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
    {
        return new Vector3D(vector1.DX + vector2.DX, vector1.DY + vector2.DY, vector1.DZ + vector2.DZ);
    }

    public override string ToString()
    {
        return $"{DX},{DY},{DZ}";
    }

    public static Vector3D Parse(string input)
    {
        var values = input.SplitToNumbers<long>(',', ' ');

        return new Vector3D(values[0], values[1], values[2]);
    }
}
