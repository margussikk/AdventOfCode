namespace AdventOfCode.Utilities.Geometry;

[Flags]
internal enum GridDirection
{
    None = 0,

    Up = 1 << 0,
    Right = 1 << 1,
    Down = 1 << 2,
    Left = 1 << 3,

    UpLeft = 1 << 4,
    UpRight = 1 << 5,
    DownLeft = 1 << 6,
    DownRight = 1 << 7,

    LeftRight = Left | Right,
    UpDown = Up | Down,
    AllSides = Up | Down | Left | Right,

    Start = 1 << 10,
    End = 1 << 11,
}