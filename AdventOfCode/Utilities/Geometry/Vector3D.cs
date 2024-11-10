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
}
