using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day16;
internal class MazeWalker
{
    public GridPosition Position { get; set; }
    public GridPosition? PreviousPosition { get; set; }

    public int Score { get; set; }

    public GridPosition[] GetNextPositions()
    {
        return [Position.TurnLeft(), Position.Move(1), Position.TurnRight()];
    }
}
