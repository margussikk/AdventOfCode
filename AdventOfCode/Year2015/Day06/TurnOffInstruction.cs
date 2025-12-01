using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2015.Day06;

internal class TurnOffInstruction : Instruction
{
    public TurnOffInstruction(GridCoordinate firstCoordinate, GridCoordinate lastCoordinate)
        : base(firstCoordinate, lastCoordinate)
    {
    }

    protected override bool GetNextValuePartOne(bool value)
    {
        return false;
    }

    protected override int GetNextValuePartTwo(int value)
    {
        return int.Max(value - 1, 0);
    }
}
