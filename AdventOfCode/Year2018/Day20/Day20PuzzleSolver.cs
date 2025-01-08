using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2018.Day20;

[Puzzle(2018, 20, "A Regular Map")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private RootElement _rootElement = new();

    public void ParseInput(string[] inputLines)
    {
        _rootElement = RootElement.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = int.MinValue;

        var startCoordinate = GridCoordinate.Zero;
        var grid = new InfiniteGrid<bool>();

        _rootElement.Walk(startCoordinate, grid);

        var pathFinder = new GridPathFinder<bool>(grid)
            .SetCellFilter((_, cell) => cell.Object);

        pathFinder.WalkAllPaths(true, startCoordinate, CountDoors);

        return new PuzzleAnswer(answer, 3806);

        bool CountDoors(GridPathWalker walker)
        {
            if (IsWalkerInTheRoom(walker))
            {
                answer = int.Max(answer, walker.Cost / 2); // / 2 because we walk 2 steps at a time
            }

            return true;
        }
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var roomCoordinates = new HashSet<GridCoordinate>();

        var startCoordinate = GridCoordinate.Zero;
        var grid = new InfiniteGrid<bool>();

        _rootElement.Walk(startCoordinate, grid);

        var pathFinder = new GridPathFinder<bool>(grid)
            .SetCellFilter((_, cell) => cell.Object);

        pathFinder.WalkAllPaths(true, startCoordinate, CountRooms);

        var answer = roomCoordinates.Count;

        return new PuzzleAnswer(answer, 8354);

        bool CountRooms(GridPathWalker walker)
        {
            if (walker.Cost >= 2 * 1000 && IsWalkerInTheRoom(walker)) // 2 * because we walk 2 steps at a time and 1000 rooms = 2000 steps
            {
                roomCoordinates.Add(walker.Position.Coordinate);
            }

            return true;
        }
    }

    private static bool IsWalkerInTheRoom(GridPathWalker walker)
    {
        return MathFunctions.Modulo(walker.Position.Coordinate.Row, 2) == 0 &&
               MathFunctions.Modulo(walker.Position.Coordinate.Column, 2) == 0;
    }
}
