using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2015.Day06;

internal abstract class Instruction
{
    public GridCoordinate FirstCoordinate { get; set; }
    public GridCoordinate LastCoordinate { get; set; }

    protected Instruction(GridCoordinate firstCoordinate, GridCoordinate lastCoordinate)
    {
        FirstCoordinate = firstCoordinate;
        LastCoordinate = lastCoordinate;
    }

    public void ExecutePartOne(Grid<bool> grid)
    {
        for (var row = FirstCoordinate.Row; row <= LastCoordinate.Row; row++)
        {
            for (var column = FirstCoordinate.Column; column <= LastCoordinate.Column; column++)
            {
                grid[row, column] = GetNextValuePartOne(grid[row, column]);
            }
        }
    }

    public void ExecutePartTwo(Grid<int> grid)
    {
        for (var row = FirstCoordinate.Row; row <= LastCoordinate.Row; row++)
        {
            for (var column = FirstCoordinate.Column; column <= LastCoordinate.Column; column++)
            {
                grid[row, column] = GetNextValuePartTwo(grid[row, column]);
            }
        }
    }

    protected abstract bool GetNextValuePartOne(bool value);
    protected abstract int GetNextValuePartTwo(int value);

    public static Instruction Parse(string input)
    {
        var parts = input.Split(' ');

        if (parts[0] == "toggle")
        {
            return new ToggleInstruction(GridCoordinate.Parse(parts[1]), GridCoordinate.Parse(parts[3]));
        }
        else if (parts[0] == "turn" && parts[1] == "on")
        {
            return new TurnOnInstruction(GridCoordinate.Parse(parts[2]), GridCoordinate.Parse(parts[4]));
        }
        else if (parts[0] == "turn" && parts[1] == "off")
        {
            return new TurnOffInstruction(GridCoordinate.Parse(parts[2]), GridCoordinate.Parse(parts[4]));
        }

        throw new InvalidOperationException("Failed to parse input");
    }
}
