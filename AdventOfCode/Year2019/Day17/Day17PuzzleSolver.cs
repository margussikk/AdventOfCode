using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Year2019.IntCode;
using System.Globalization;
using System.Text;

namespace AdventOfCode.Year2019.Day17;

[Puzzle(2019, 17, "Set and Forget")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();

    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = BuildGrid();

        var answer = 0;

        foreach(var cell in grid)
        {
            if (cell.Object == Tile.Scaffold &&
                grid.SideNeighbors(cell.Coordinate)
                    .Count(c => c.Object == Tile.Scaffold) == 4)
            {
                answer += cell.Coordinate.Row * cell.Coordinate.Column;
            }
        }

        return new PuzzleAnswer(answer, 4800);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var actions = new List<string>();

        var grid = BuildGrid();

        var initialCoordinate = grid.FindCoordinate(tile => tile is Tile.RobotFacingUp or Tile.RobotFacingDown or Tile.RobotFacingLeft or Tile.RobotFacingRight)
            ?? throw new InvalidOperationException("Start coordinate not found");

        var initialDirection = grid[initialCoordinate] switch
        {
            Tile.RobotFacingUp => GridDirection.Up,
            Tile.RobotFacingDown => GridDirection.Down,
            Tile.RobotFacingLeft => GridDirection.Left,
            Tile.RobotFacingRight => GridDirection.Right,
            _ => throw new InvalidOperationException("Invalid initial direction")
        };

        // Build actions
        var gridWalker = new GridWalker(initialCoordinate, initialCoordinate, initialDirection, 0);

        while (true)
        {
            // Turn
            var neighbors = grid.SideNeighbors(gridWalker.CurrentCoordinate)
                                .Where(cell => cell.Object == Tile.Scaffold)
                                .ToList();

            var nextDirection = GridDirection.None;

            foreach (var neighbor in neighbors)
            {
                var direction = gridWalker.CurrentCoordinate.DirectionToward(neighbor.Coordinate);
                if (gridWalker.Direction.TurnLeft() == direction)
                {
                    nextDirection = direction;

                    actions.Add("L");
                    break;
                }
                else if (gridWalker.Direction.TurnRight() == direction)
                {
                    nextDirection = direction;

                    actions.Add("R");
                    break;
                }
                else
                {
                    // Ignore, robot came from there
                }
            }

            if (nextDirection == GridDirection.None)
            {
                break;
            }

            // Move
            var startCoordinate = gridWalker.CurrentCoordinate;
            var endCoordinate = gridWalker.CurrentCoordinate;

            while (true)
            {
                var nextCoordinate = endCoordinate.Move(nextDirection);

                if (grid.InBounds(nextCoordinate) && grid[nextCoordinate] == Tile.Scaffold)
                {
                    endCoordinate = nextCoordinate;
                }
                else
                {
                    break;
                }
            }

            var steps = MeasurementFunctions.ManhattanDistance(startCoordinate, endCoordinate);
            actions.Add(steps.ToString(CultureInfo.InvariantCulture));

            gridWalker.Move(nextDirection, steps);
        }

        var result = SplitIntoFunctions(actions, 0, [], []);
        if (!result.Success)
        {
            throw new InvalidOperationException("Failed to recurse actions");
        }

        var computer = new IntCodeComputer();
        computer.Load(_program);
        computer.WriteMemory(0, 2);

        var movementRoutine = string.Join(",", result.MainRoutine) + '\n';
        foreach (var character in movementRoutine)
        {
            computer.Inputs.Enqueue(Convert.ToInt64(character));
        }

        var movementFunctions = string.Join(",", result.Functions["A"].Actions) + "\n" +
                                string.Join(",", result.Functions["B"].Actions) + "\n" +
                                string.Join(",", result.Functions["C"].Actions) + "\n";
        foreach (var character in movementFunctions)
        {
            computer.Inputs.Enqueue(Convert.ToInt64(character));
        }

        var continuousVideoFeed = "n\n";
        foreach (var character in continuousVideoFeed)
        {
            computer.Inputs.Enqueue(Convert.ToInt64(character));
        }

        computer.Run();

        var answer = computer.Outputs.Last();

        return new PuzzleAnswer(answer, 982279);
    }

    private Grid<Tile> BuildGrid()
    {
        var computer = new IntCodeComputer();
        computer.Load(_program);
        computer.Run();

        var stringBuilder = new StringBuilder();

        while (computer.Outputs.TryDequeue(out var output))
        {
            stringBuilder.Append(Convert.ToChar(output));
        }

        var grid = stringBuilder
            .ToString()
            .Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray()
            .SelectToGrid(character => character switch
            {
                '.' => Tile.OpenSpace,
                '#' => Tile.Scaffold,
                '^' => Tile.RobotFacingUp,
                'v' => Tile.RobotFacingDown,
                '<' => Tile.RobotFacingLeft,
                '>' => Tile.RobotFacingRight,
                _ => throw new InvalidOperationException($"Invalid tile character {character}")
            });

        return grid;
    }

    private static SplitResult SplitIntoFunctions(List<string> actions, int actionIndex, List<string> mainRoutine, Dictionary<string, Function> functions)
    {
        if (actionIndex >= actions.Count)
        {
            if (functions.Count == 3)
            {
                return new SplitResult
                {
                    Success = true,
                    MainRoutine = mainRoutine,
                    Functions = functions
                };
            }
            else
            {
                return new SplitResult
                {
                    Success = false
                };
            }
        }

        for (var length = 2; actionIndex + length <= actions.Count; length += 2) // 2 = Handle turn and movement together
        {
            var newMainRoutine = new List<string>(mainRoutine);
            var newFunctions = new Dictionary<string, Function>(functions);

            var pattern = actions.GetRange(actionIndex, length);

            var patternLength = pattern.Sum(x => x.Length) + pattern.Count - 1;
            if (patternLength >= 20) // Elements + commas
            {
                continue;
            }

            var fun = functions.FirstOrDefault(x => x.Value.Actions.SequenceEqual(pattern));
            var function = fun.Key != null ? fun.Value : null;
            if (function == null)
            {
                if (newFunctions.Count == 3) // Only A, B, C allowed
                {
                    continue;
                }

                function = new Function(Convert.ToChar('A' + functions.Count).ToString(), pattern);
                newFunctions[function.Name] = function;
            }

            newMainRoutine.Add(function.Name);
            
            var newMainRoutineLength = newMainRoutine.Count + (newMainRoutine.Count - 1); // Elements + commas
            if (newMainRoutineLength >= 20)
            {
                continue;
            }

            var result = SplitIntoFunctions(actions, actionIndex + length, newMainRoutine, newFunctions);
            if (result.Success)
            {
                return result;
            }
        }

        return new SplitResult
        {
            Success = false
        };
    }

    private sealed class SplitResult
    {
        public bool Success { get; init; }

        public IEnumerable<string> MainRoutine { get; init; } = [];

        public Dictionary<string, Function> Functions { get; init; } = [];
    }
}