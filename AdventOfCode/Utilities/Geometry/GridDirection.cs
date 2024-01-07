namespace AdventOfCode.Utilities.Geometry;

[Flags]
internal enum GridDirection
{
    None = 0,

    Up = 1 << 0,
    Right = 1 << 1,
    Down = 1 << 2,
    Left = 1 << 3,

    Start = 1 << 4,
    End = 1 << 5,
}