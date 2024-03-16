using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Utilities.Geometry;

internal readonly struct Vector2D(long dX, long dY)
{
    public static readonly Vector2D UnitX = new(1, 0);

    public static readonly Vector2D UnitY = new(0, 1);

    public long DX { get; } = dX;

    public long DY { get; } = dY;

    public static Vector2D operator *(Vector2D vector, double scalar)
    {
        return new Vector2D(Convert.ToInt64(vector.DX * scalar), Convert.ToInt64(vector.DY * scalar));
    }

    public Vector2D Normalize()
    {
        var gcm = MathFunctions.GreatestCommonDivisor(Math.Abs(DX), Math.Abs(DY));
        return new Vector2D(DX / gcm, DY / gcm);
    }
}
