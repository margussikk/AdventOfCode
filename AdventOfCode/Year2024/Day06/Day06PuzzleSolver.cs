using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day06;

[Puzzle(2024, 6, "Guard Gallivant")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private BitGrid _obstacleGrid = new(0, 0);
    private Guard _initialGuard = new();

    public void ParseInput(string[] inputLines)
    {
        _obstacleGrid = new BitGrid(inputLines.Length, inputLines[0].Length);

        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                var symbol = inputLines[row][column];

                if (symbol == '#')
                {
                    _obstacleGrid[row, column] = true;
                }
                else if (symbol == '^')
                {
                    _initialGuard = new Guard
                    {
                        Coordinate = new GridCoordinate(row, column),
                        Direction = GridDirection.Up
                    };
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetVisitedCoordinates().Count;

        return new PuzzleAnswer(answer, 4973);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var obstacleCoordinates = new HashSet<GridCoordinate>();

        foreach (var coordinate in GetVisitedCoordinates())
        {
            var hitObstaclesGrid = new Grid<GridDirection>(_obstacleGrid.Height, _obstacleGrid.Width);
            var guard = new Guard
            {
                Coordinate = _initialGuard.Coordinate,
                Direction = _initialGuard.Direction,
            };

            while (true)
            {
                if (!TryMoveGuardFast(guard, coordinate, out var hitObstacleCoordinate))
                {
                    break;
                }

                if (hitObstaclesGrid[hitObstacleCoordinate].HasFlag(guard.Direction))
                {
                    obstacleCoordinates.Add(coordinate);
                    break;
                }

                hitObstaclesGrid[hitObstacleCoordinate] |= guard.Direction;
            }
        }

        var answer = obstacleCoordinates.Count;

        return new PuzzleAnswer(answer, 1482);
    }

    private List<GridCoordinate> GetVisitedCoordinates()
    {
        var visited = new HashSet<GridCoordinate>();

        var guard = new Guard
        {
            Coordinate = _initialGuard.Coordinate,
            Direction = _initialGuard.Direction,
        };

        while (true)
        {
            visited.Add(guard.Coordinate);

            if (!TryMoveGuardSlow(guard))
            {
                break;
            }
        }

        return [.. visited];
    }

    private bool TryMoveGuardSlow(Guard guard)
    {
        var nextDirection = guard.Direction;
        var nextCoordinate = guard.Coordinate.Move(nextDirection);

        if (!_obstacleGrid.InBounds(nextCoordinate))
        {
            return false;
        }
        
        while(_obstacleGrid[nextCoordinate])
        {
            nextDirection = nextDirection.TurnRight();
            nextCoordinate = guard.Coordinate.Move(nextDirection);
        }

        guard.Direction = nextDirection;
        guard.Coordinate = nextCoordinate;

        return true;
    }

    private bool TryMoveGuardFast(Guard guard, GridCoordinate obstacleCoordinate, out GridCoordinate hitObstacleCoordinate)
    {
        while (true)
        {
            var nextCoordinate = guard.Coordinate.Move(guard.Direction);

            if (!_obstacleGrid.InBounds(nextCoordinate))
            {
                hitObstacleCoordinate = default;
                return false;
            }

            if (_obstacleGrid[nextCoordinate])
            {
                hitObstacleCoordinate = nextCoordinate;
                guard.Direction = guard.Direction.TurnRight();
                return true;
            }

            if (nextCoordinate == obstacleCoordinate)
            {
                hitObstacleCoordinate = obstacleCoordinate;
                guard.Direction = guard.Direction.TurnRight();
                return true;
            }

            guard.Coordinate = nextCoordinate;
        }
    }
}