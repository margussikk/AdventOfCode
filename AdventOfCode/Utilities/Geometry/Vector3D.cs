namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Vector3D(long dX, long dY, long dZ)
{
    public static readonly Vector3D UnitX = new(1, 0, 0);

    public static readonly Vector3D UnitY = new(0, 1, 0);

    public static readonly Vector3D UnitZ = new(0, 0, 1);

    public long DX { get; } = dX;

    public long DY { get; } = dY;

    public long DZ { get; } = dZ;
}
