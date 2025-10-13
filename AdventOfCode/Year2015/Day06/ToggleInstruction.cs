using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2015.Day06;
internal class ToggleInstruction : Instruction
{
    public ToggleInstruction(GridCoordinate firstCoordinate, GridCoordinate lastCoordinate)
        : base(firstCoordinate, lastCoordinate)
    {
    }

    protected override bool GetNextValuePartOne(bool value)
    {
        return !value;
    }

    protected override int GetNextValuePartTwo(int value)
    {
        return value + 2;
    }
}
