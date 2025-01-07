using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2024.Day06;

[Puzzle(2024, 6, "Guard Gallivant")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private BitGrid _obstacleGrid = new(0, 0);
    private GridPosition _initialGuardPosition = new();

    public void ParseInput(string[] inputLines)
    {
        _obstacleGrid = inputLines.SelectToBitGrid((character, coordinate) =>
        {
            if (character.IsGridDirection())
            {
                _initialGuardPosition = new GridPosition(coordinate, character.ParseToGridDirection());
            }

            return character == '#';
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetVisitedCoordinates().Count;

        return new PuzzleAnswer(answer, 4973);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        Parallel.ForEach(GetVisitedCoordinates(), coordinate =>
        {
            var hitObstaclesGrid = new Grid<GridDirection>(_obstacleGrid.Height, _obstacleGrid.Width);
            var guard = new GridWalker(_initialGuardPosition);

            while (true)
            {
                if (!TryMoveGuardFast(ref guard, coordinate, out var hitObstacleCoordinate))
                {
                    break;
                }

                if (hitObstaclesGrid[hitObstacleCoordinate].HasFlag(guard.Direction))
                {
                    Interlocked.Increment(ref answer);
                    break;
                }

                hitObstaclesGrid[hitObstacleCoordinate] |= guard.Direction;
            }
        });

        return new PuzzleAnswer(answer, 1482);
    }

    private List<GridCoordinate> GetVisitedCoordinates()
    {
        var visited = new HashSet<GridCoordinate>();

        var guard = new GridWalker(_initialGuardPosition);

        do
        {
            visited.Add(guard.Coordinate);
        }
        while (TryMoveGuardSlow(ref guard));

        return [.. visited];
    }

    private bool TryMoveGuardSlow(ref GridWalker guard)
    {
        var nextCoordinate = guard.Coordinate.Move(guard.Direction);

        if (!_obstacleGrid.InBounds(nextCoordinate))
        {
            return false;
        }

        while (_obstacleGrid[nextCoordinate])
        {
            guard.TurnRight();
            nextCoordinate = guard.Coordinate.Move(guard.Direction);
        }

        guard.Step();

        return true;
    }

    private bool TryMoveGuardFast(ref GridWalker guard, GridCoordinate obstacleCoordinate, out GridCoordinate hitObstacleCoordinate)
    {
        while (true)
        {
            var nextCoordinate = guard.Coordinate.Move(guard.Direction);

            if (!_obstacleGrid.InBounds(nextCoordinate))
            {
                hitObstacleCoordinate = default;
                return false;
            }

            if (_obstacleGrid[nextCoordinate] || nextCoordinate == obstacleCoordinate)
            {
                hitObstacleCoordinate = nextCoordinate;
                guard.TurnRight();
                return true;
            }

            guard.Step();
        }
    }
}