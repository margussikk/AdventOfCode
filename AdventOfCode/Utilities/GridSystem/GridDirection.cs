namespace AdventOfCode.Utilities.GridSystem;

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

    LeftAndRight = Left | Right,
    UpAndDown = Up | Down,
    UpAndLeft = Up | Left,
    DownAndLeft = Down | Left,
    UpAndRight = Up | Right,
    DownAndRight = Down | Right,
    AllSides = Up | Down | Left | Right,
}