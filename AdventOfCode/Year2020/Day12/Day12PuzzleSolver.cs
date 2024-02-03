using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2020.Day12;

[Puzzle(2020, 12, "Rain Risk")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var startCoordinate = new GridCoordinate(0, 0);

        var ship = new Ship
        {
            Coordinate = startCoordinate,
            Direction = GridDirection.Right
        };

        foreach (var instruction in _instructions)
        {
            switch (instruction.InstructionType)
            {
                case InstructionType.North:
                    ship.Coordinate = ship.Coordinate.Move(GridDirection.Up, instruction.Argument);
                    break;
                case InstructionType.South:
                    ship.Coordinate = ship.Coordinate.Move(GridDirection.Down, instruction.Argument);
                    break;
                case InstructionType.East:
                    ship.Coordinate = ship.Coordinate.Move(GridDirection.Right, instruction.Argument);
                    break;
                case InstructionType.West:
                    ship.Coordinate = ship.Coordinate.Move(GridDirection.Left, instruction.Argument);
                    break;
                case InstructionType.Forward:
                    ship.Coordinate = ship.Coordinate.Move(ship.Direction, instruction.Argument);
                    break;
                case InstructionType.TurnLeft:
                    for (var times = instruction.Argument; times > 0; times -= 90)
                    {
                        ship.Direction = ship.Direction.TurnLeft();
                    }
                    break;
                case InstructionType.TurnRight:
                    for (var times = instruction.Argument; times > 0; times -= 90)
                    {
                        ship.Direction = ship.Direction.TurnRight();
                    }
                    break;
            }
        }

        var answer = MeasurementFunctions.ManhattanDistance(startCoordinate, ship.Coordinate);

        return new PuzzleAnswer(answer, 2458);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var startCoordinate = new GridCoordinate(0, 0);

        var ship = new Ship
        {
            Coordinate = startCoordinate,
            Direction = GridDirection.Right
        };

        var waypoint = new GridCoordinate(-1, 10);

        foreach (var instruction in _instructions)
        {
            switch (instruction.InstructionType)
            {
                case InstructionType.North:
                    waypoint = waypoint.Move(GridDirection.Up, instruction.Argument);
                    break;
                case InstructionType.South:
                    waypoint = waypoint.Move(GridDirection.Down, instruction.Argument);
                    break;
                case InstructionType.East:
                    waypoint = waypoint.Move(GridDirection.Right, instruction.Argument);
                    break;
                case InstructionType.West:
                    waypoint = waypoint.Move(GridDirection.Left, instruction.Argument);
                    break;
                case InstructionType.Forward:
                    ship.Coordinate = new GridCoordinate(ship.Coordinate.Row + instruction.Argument * waypoint.Row, ship.Coordinate.Column + instruction.Argument * waypoint.Column);
                    break;
                case InstructionType.TurnLeft:
                    for (var times = instruction.Argument; times > 0; times -= 90)
                    {
                        waypoint = waypoint.RotateCounterClockwise();
                    }
                    break;
                case InstructionType.TurnRight:
                    for (var times = instruction.Argument; times > 0; times -= 90)
                    {
                        waypoint = waypoint.RotateClockwise();
                    }
                    break;
            }
        }

        var answer = MeasurementFunctions.ManhattanDistance(startCoordinate, ship.Coordinate);

        return new PuzzleAnswer(answer, 145117);
    }
}