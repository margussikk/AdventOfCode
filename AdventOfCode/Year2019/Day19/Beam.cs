using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day19;

internal class Beam
{
    public Coordinate2D Start { get; set; }
    public Vector2D Vector { get; set; }

    public long GetX(long y)
    {
        var x = Math.Ceiling(1D * (y - Start.Y) * Vector.DX / Vector.DY + Start.X);

        return Convert.ToInt64(x);
    }
}
