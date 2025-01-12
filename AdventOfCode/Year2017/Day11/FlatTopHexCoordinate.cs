using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2017.Day11;
internal class FlatTopHexCoordinate
{
    public int Q { get; }

    public int R { get; }

    public int S => -Q - R;

    public FlatTopHexCoordinate(int q, int r)
    {
        Q = q;
        R = r;
    }

    public FlatTopHexCoordinate Move(GridDirection direction)
    {
        return direction switch
        {
            GridDirection.UpLeft => new FlatTopHexCoordinate(Q - 1, R),
            GridDirection.Up => new FlatTopHexCoordinate(Q, R - 1),
            GridDirection.UpRight => new FlatTopHexCoordinate(Q + 1, R - 1),

            GridDirection.DownLeft => new FlatTopHexCoordinate(Q - 1, R + 1),
            GridDirection.Down => new FlatTopHexCoordinate(Q, R + 1),
            GridDirection.DownRight => new FlatTopHexCoordinate(Q + 1, R),

            GridDirection.None => this,
            _ => throw new InvalidOperationException("Unexpected direction")
        };
    }
}
