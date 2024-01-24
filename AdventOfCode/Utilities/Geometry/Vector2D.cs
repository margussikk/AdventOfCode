namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Vector2D(long dX, long dY)
{
    public static readonly Vector2D UnitX = new(1, 0);

    public static readonly Vector2D UnitY = new(0, 1);

    public long DX { get; } = dX;

    public long DY { get; } = dY;
}
