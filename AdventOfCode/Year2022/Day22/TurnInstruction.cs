using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2022.Day22;

internal class TurnInstruction : IInstruction
{
    public GridDirection Direction { get; }

    public TurnInstruction(GridDirection direction)
    {
        Direction = direction;
    }
}
