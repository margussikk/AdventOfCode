namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Vector2D(long x, long y, long dX, long dY)
{
    public static readonly Vector2D UnitX = new(0, 0, 1, 0);

    public static readonly Vector2D UnitY = new(0, 0, 0, 1);

    public long X { get; } = x;

    public long Y { get; } = y;

    public long DX { get; } = dX;

    public long DY { get; } = dY;
}
