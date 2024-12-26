using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Utilities.Geometry;

internal readonly record struct Vector2D
{
    public static readonly Vector2D UnitX = new(1, 0);

    public static readonly Vector2D UnitY = new(0, 1);

    public long DX { get; }

    public long DY { get; }

    public Vector2D(long dX, long dY)
    {
        DX = dX;
        DY = dY;
    }

    public Vector2D Normalize()
    {
        var gcm = MathFunctions.GreatestCommonDivisor(Math.Abs(DX), Math.Abs(DY));
        return new Vector2D(DX / gcm, DY / gcm);
    }

    public static Vector2D operator *(Vector2D vector, double scalar)
    {
        return new Vector2D(Convert.ToInt64(vector.DX * scalar), Convert.ToInt64(vector.DY * scalar));
    }

    public static Vector2D Parse(string input)
    {
        var splits = input.Split(',');

        return new Vector2D(long.Parse(splits[0]), long.Parse(splits[1]));
    }
}
