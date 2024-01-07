namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Vector2D(long x, long y, long dX, long dY)
{
    public long X { get; } = x;

    public long Y { get; } = y;

    public long DX { get; } = dX;

    public long DY { get; } = dY;
}
