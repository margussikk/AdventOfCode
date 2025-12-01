using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2015.Day06;

internal class TurnOnInstruction : Instruction
{
    public TurnOnInstruction(GridCoordinate firstCoordinate, GridCoordinate lastCoordinate)
        : base(firstCoordinate, lastCoordinate)
    {
    }

    protected override bool GetNextValuePartOne(bool value)
    {
        return true;
    }

    protected override int GetNextValuePartTwo(int value)
    {
        return value + 1;
    }
}
