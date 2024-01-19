using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day22;

internal class TurnInstruction(GridDirection direction) : IInstruction
{
    public GridDirection Direction { get; } = direction;
}
