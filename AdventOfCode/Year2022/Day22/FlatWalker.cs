using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2022.Day22;

internal class FlatWalker(Grid<Tile> grid)
{
    public GridCoordinate Location { get; private set; } =
        grid.FindCoordinate(tile => tile == Tile.Open) ?? throw new InvalidOperationException("Flat walker stat location not found");

    public GridDirection Direction { get; private set; } = GridDirection.Right;

    private readonly Grid<Tile> _grid = grid;

    private readonly int _cubeEdgeLength = int.Max(grid.Height, grid.Width) % int.Min(grid.Height, grid.Width);

    public void TurnLeft()
    {
        Direction = Direction.TurnLeft();
    }

    public void TurnRight()
    {
        Direction = Direction.TurnRight();
    }

    public void Move(int steps)
    {
        for (var step = 0; step < steps; step++)
        {
            var nextLocation = Location;

            // Move 1 step
            nextLocation = nextLocation.Move(Direction);
            if (!_grid.InBounds(nextLocation))
            {
                var nextRow = MathFunctions.Modulo(nextLocation.Row, _grid.Height);
                var nextColumn = MathFunctions.Modulo(nextLocation.Column, _grid.Width);

                nextLocation = new GridCoordinate(nextRow, nextColumn);
            }

            // Little speed up, skip multiple nothing tiles at once
            while (_grid[nextLocation] == Tile.Nothing)
            {
                nextLocation = nextLocation.Move(Direction, _cubeEdgeLength);
                if (!_grid.InBounds(nextLocation))
                {
                    var nextRow = MathFunctions.Modulo(nextLocation.Row, _grid.Height);
                    var nextColumn = MathFunctions.Modulo(nextLocation.Column, _grid.Width);

                    nextLocation = new GridCoordinate(nextRow, nextColumn);
                }
            }

            if (_grid[nextLocation] == Tile.Wall)
            {
                // Location stays the same
                break;
            }
            else if (_grid[nextLocation] == Tile.Open)
            {
                Location = nextLocation;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
