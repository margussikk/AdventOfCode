using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.PathFinding;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day15;

[Puzzle(2019, 15, "Oxygen System")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SplitToNumbers<long>(',');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = BuildGrid();

        var startCoordinate = grid.FindCoordinate(tile => tile == Tile.Start)
            ?? throw new InvalidOperationException("Start coordinate not found");
        var oxygenSystemCoordinate = grid.FindCoordinate(tile => tile == Tile.OxygenSystem)
            ?? throw new InvalidOperationException("Oxygen system coordinate not found");

        var gridPathFinder = new GridPathFinder<Tile>(grid)
            .SetCellFilter((w, c) => c.Object != Tile.Wall);

        var answer = gridPathFinder.FindShortestPathLength(startCoordinate, oxygenSystemCoordinate);

        return new PuzzleAnswer(answer, 204);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var grid = BuildGrid();

        var oxygenSystemCoordinate = grid.FindCoordinate(tile => tile == Tile.OxygenSystem)
            ?? throw new InvalidOperationException("Oxygen system coordinate not found");

        var visited = new HashSet<GridCoordinate>();

        var oxygenSpreaders = new List<GridCoordinate> { oxygenSystemCoordinate };

        var answer = -1;

        while (oxygenSpreaders.Count > 0)
        {
            var newOxygenSpreaders = new List<GridCoordinate>();

            foreach (var oxygenSpreader in oxygenSpreaders.Where(os => visited.Add(os)))
            {
                var neighborCoordinates = grid
                    .SideNeighbors(oxygenSpreader)
                    .Where(cell => cell.Object != Tile.Wall && !visited.Contains(cell.Coordinate))
                    .Select(cell => cell.Coordinate);

                newOxygenSpreaders.AddRange(neighborCoordinates);
            }

            oxygenSpreaders = newOxygenSpreaders;

            answer++;
        }

        return new PuzzleAnswer(answer, 340);
    }

    private static long GetRobotDirection(GridDirection direction)
    {
        return direction switch
        {
            GridDirection.Up => 1,
            GridDirection.Down => 2,
            GridDirection.Left => 3,
            GridDirection.Right => 4,
            _ => throw new InvalidOperationException($"Invalid direction {direction}")
        };
    }

    private static void Print(Dictionary<GridCoordinate, Tile> map)
    {
        var minRow = map.Min(kvp => kvp.Key.Row);
        var maxRow = map.Max(kvp => kvp.Key.Row);

        var minColumn = map.Min(kvp => kvp.Key.Column);
        var maxColumn = map.Max(kvp => kvp.Key.Column);

        for (var row = minRow; row <= maxRow; row++)
        {
            for (var column = minColumn; column <= maxColumn; column++)
            {
                var coordinate = new GridCoordinate(row, column);
                var tile = map.GetValueOrDefault(coordinate, Tile.Wall);

                var character = tile switch
                {
                    Tile.Empty => ' ',
                    Tile.Wall => '#',
                    Tile.OxygenSystem => 'O',
                    Tile.Start => 'S',
                    _ => throw new InvalidOperationException($"Invalid tile {tile}")
                };

                Console.Write(character);
            }

            Console.WriteLine();
        }
    }

    private Grid<Tile> BuildGrid()
    {
        var minRow = int.MaxValue / 2;
        var maxRow = int.MinValue / 2;

        var minColumn = int.MaxValue / 2;
        var maxColumn = int.MinValue / 2;

        var computer = new IntCodeComputer(_program);

        var map = new Dictionary<GridCoordinate, Tile>();

        var currentCoordinate = new GridCoordinate(0, 0);
        map[currentCoordinate] = Tile.Start;

        var stack = new Stack<GridDirection>();
        stack.Push(GridDirection.None);

        while (stack.Count > 0)
        {
            var nextCoordinate = currentCoordinate.SideNeighbors()
                .Cast<GridCoordinate?>()
                .FirstOrDefault(coordinate => coordinate.HasValue && !map.ContainsKey(coordinate.Value));

            if (nextCoordinate is not null)
            {
                var direction = currentCoordinate.DirectionToward(nextCoordinate.Value);
                var robotDirection = GetRobotDirection(direction);

                var result = computer.Run(robotDirection);

                var output = (Tile)result.Outputs[0];
                if (output is Tile.Empty or Tile.OxygenSystem)
                {
                    currentCoordinate = nextCoordinate.Value;
                    stack.Push(direction);
                }

                map[nextCoordinate.Value] = output;

                minRow = int.Min(nextCoordinate.Value.Row, minRow);
                maxRow = int.Max(nextCoordinate.Value.Row, maxRow);

                minColumn = int.Min(nextCoordinate.Value.Column, minColumn);
                maxColumn = int.Max(nextCoordinate.Value.Column, maxColumn);
            }
            else
            {
                var direction = stack.Pop();
                if (direction == GridDirection.None) continue;

                var backDirection = direction.Flip();
                var robotDirection = GetRobotDirection(backDirection);

                var result = computer.Run(robotDirection);

                var output = (Tile)result.Outputs[0];
                if (output == Tile.Wall)
                {
                    throw new InvalidOperationException("Shouldn't hit the wall");
                }

                currentCoordinate = currentCoordinate.Move(backDirection);
            }
        }

        var grid = new Grid<Tile>(maxRow - minRow + 1, maxColumn - minColumn + 1);

        for (var row = 0; row <= grid.LastRow; row++)
        {
            for (var column = 0; column <= grid.LastColumn; column++)
            {
                var coordinate = new GridCoordinate(row + minRow, column + minColumn);
                var tile = map.GetValueOrDefault(coordinate, Tile.Wall);

                grid[row, column] = tile;
            }
        }

        return grid;
    }
}