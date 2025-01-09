using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Simulation;
using Iced.Intel;
using Spectre.Console;

namespace AdventOfCode.Year2020.Day11;

[Puzzle(2020, 11, "Seating System")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(character => character switch
        {
            '.' => Tile.Floor,
            'L' => Tile.EmptySeat,
            '#' => Tile.OccupiedSeat,
            _ => throw new InvalidOperationException("Invalid tile character")
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var gameOfLife = new GameOfLife<Tile>(_grid.Height, _grid.Width);

        foreach (var cell in _grid)
        {
            gameOfLife.SetState(cell.Coordinate, cell.Object);
        }

        bool seatChangedState;
        do
        {
            seatChangedState = false;
            var nextGameOfLife = gameOfLife.Clone();

            foreach (var cell in gameOfLife)
            {
                if (cell.Object == Tile.EmptySeat && cell.AroundCount(Tile.OccupiedSeat) == 0)
                {
                    nextGameOfLife.SetState(cell.Coordinate, Tile.OccupiedSeat);
                    seatChangedState = true;
                }
                else if (cell.Object == Tile.OccupiedSeat && cell.AroundCount(Tile.OccupiedSeat) >= 4)
                {
                    nextGameOfLife.SetState(cell.Coordinate, Tile.EmptySeat);
                    seatChangedState = true;
                }
            }

            gameOfLife = nextGameOfLife;
        }
        while (seatChangedState);

        var answer = gameOfLife.Count(Tile.OccupiedSeat);

        return new PuzzleAnswer(answer, 2126);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var directions = new[]
        {
            GridDirection.UpLeft, GridDirection.Up, GridDirection.UpRight,
            GridDirection.Left, GridDirection.Right,
            GridDirection.DownLeft, GridDirection.Down, GridDirection.DownRight
        };

        var grid = _grid.Clone();

        var updatedGridCells = new List<GridCell<Tile>>();

        do
        {
            updatedGridCells.Clear();

            foreach (var gridCell in grid)
            {
                switch (gridCell.Object)
                {
                    case Tile.EmptySeat:
                        {
                            var occupiedSeatCount = directions.Count(direction => SeesOccupiedSeat(grid, gridCell.Coordinate, direction));
                            if (occupiedSeatCount == 0)
                            {
                                updatedGridCells.Add(new GridCell<Tile>(gridCell.Coordinate, Tile.OccupiedSeat));
                            }

                            break;
                        }
                    case Tile.OccupiedSeat:
                        {
                            var occupiedSeatCount = directions.Count(direction => SeesOccupiedSeat(grid, gridCell.Coordinate, direction));
                            if (occupiedSeatCount >= 5)
                            {
                                updatedGridCells.Add(new GridCell<Tile>(gridCell.Coordinate, Tile.EmptySeat));
                            }

                            break;
                        }
                }
            }

            foreach (var updatedGridCell in updatedGridCells)
            {
                grid[updatedGridCell.Coordinate] = updatedGridCell.Object;
            }

        } while (updatedGridCells.Count > 0);

        var answer = grid.Count(gc => gc.Object == Tile.OccupiedSeat);

        return new PuzzleAnswer(answer, 1914);
    }

    private static bool SeesOccupiedSeat(Grid<Tile> grid, GridCoordinate coordinate, GridDirection direction)
    {
        var testCoordinate = coordinate;

        while (grid.InBounds(testCoordinate))
        {
            testCoordinate = testCoordinate.Move(direction);

            if (!grid.InBounds(testCoordinate))
            {
                return false;
            }

            switch (grid[testCoordinate])
            {
                case Tile.OccupiedSeat:
                    return true;
                case Tile.EmptySeat:
                    return false;
            }
        }

        return false;
    }
}