using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.GridSystem;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day17;

[Puzzle(2018, 17, "Reservoir Research")]
public partial class Day17PuzzleSolver : IPuzzleSolver
{
    private readonly List<LineSegment2D> _clayVeins = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var matches = ButtonInputLineRegex().Matches(line);
            if (matches.Count != 1)
            {
                throw new InvalidOperationException("Failed to parse input line");
            }

            var match = matches[0];

            var fixedValue = long.Parse(match.Groups[2].Value);
            var startValue = long.Parse(match.Groups[4].Value);
            var endValue = long.Parse(match.Groups[5].Value);

            if (match.Groups[1].Value == "x" && match.Groups[3].Value == "y")
            {
                var startCoordinate = new Coordinate2D(fixedValue, startValue);
                var endCoordinate = new Coordinate2D(fixedValue, endValue);

                _clayVeins.Add(new LineSegment2D(startCoordinate, endCoordinate));
            }
            else if (match.Groups[1].Value == "y" && match.Groups[3].Value == "x")
            {
                var startCoordinate = new Coordinate2D(startValue, fixedValue);
                var endCoordinate = new Coordinate2D(endValue, fixedValue);

                _clayVeins.Add(new LineSegment2D(startCoordinate, endCoordinate));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var waterGrid = ProduceWaterGrid();

        var answer = waterGrid.Count(c => c.Object == Tile.WetSand || c.Object == Tile.Water);

        return new PuzzleAnswer(answer, 31949);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var waterGrid = ProduceWaterGrid();

        var answer = waterGrid.Count(c => c.Object == Tile.Water);

        return new PuzzleAnswer(answer, 26384);
    }


    private Grid<Tile> ProduceWaterGrid()
    {
        var region = new Region2D(_clayVeins);

        var minX = region.MinCoordinate.X - 1;
        var maxX = region.MaxCoordinate.X + 1;

        var minY = region.MinCoordinate.Y;
        var maxY = region.MaxCoordinate.Y;

        var grid = new Grid<Tile>(Convert.ToInt32(maxY - minY + 1), Convert.ToInt32(maxX - minX + 1));

        foreach (var coordinate in _clayVeins.SelectMany(x => x))
        {
            grid[Convert.ToInt32(coordinate.Y - minY), Convert.ToInt32(coordinate.X - minX)] = Tile.Clay;
        }

        var waterCoordinate = new GridCoordinate(0, Convert.ToInt32(500 - minX));

        var waterStack = new Stack<GridCoordinate>();
        waterStack.Push(waterCoordinate);

        while (waterStack.TryPop(out waterCoordinate))
        {
            if (waterCoordinate.Row == grid.LastRow)
            {
                grid[waterCoordinate] = Tile.WetSand;
                continue;
            }
            else if (grid[waterCoordinate] == Tile.Water)
            {
                continue;
            }

            var downCoordinate = waterCoordinate.Down();
            if (grid[downCoordinate] == Tile.WetSand)
            {
                grid[waterCoordinate] = Tile.WetSand;
            }
            else if (grid[downCoordinate] == Tile.Sand)
            {
                grid[waterCoordinate] = Tile.WetSand;

                waterStack.Push(waterCoordinate);
                waterStack.Push(downCoordinate);
            }
            else if (IsFloor(downCoordinate))
            {
                // Left
                var foundLeftBarrier = true;
                var leftBarrierCoordinate = waterCoordinate;
                while (IsSand(leftBarrierCoordinate) && IsFloor(leftBarrierCoordinate.Down()))
                {
                    leftBarrierCoordinate = leftBarrierCoordinate.Left();
                }

                if (grid[leftBarrierCoordinate] != Tile.Clay || !IsFloor(leftBarrierCoordinate.Down()))
                {
                    foundLeftBarrier = false;
                }

                // Right
                var foundRightBarrier = true;
                var rightBarrierCoordinate = waterCoordinate;
                while (IsSand(rightBarrierCoordinate) && IsFloor(rightBarrierCoordinate.Down()))
                {
                    rightBarrierCoordinate = rightBarrierCoordinate.Right();
                }

                if (grid[rightBarrierCoordinate] != Tile.Clay || !IsFloor(rightBarrierCoordinate.Down()))
                {
                    foundRightBarrier = false;
                }

                // If found both left and right barrier, then we have found a reservoir
                if (foundLeftBarrier && foundRightBarrier)
                {
                    for (var column = leftBarrierCoordinate.Column + 1; column < rightBarrierCoordinate.Column; column++)
                    {
                        grid[waterCoordinate.Row, column] = Tile.Water;
                    }
                }
                else
                {
                    for (var column = leftBarrierCoordinate.Column + 1; column < rightBarrierCoordinate.Column; column++)
                    {
                        grid[waterCoordinate.Row, column] = Tile.WetSand;
                    }

                    if (!foundLeftBarrier)
                    {
                        waterStack.Push(leftBarrierCoordinate);
                    }

                    if (!foundRightBarrier)
                    {
                        waterStack.Push(rightBarrierCoordinate);
                    }
                }
            }
        }

        return grid;

        bool IsSand(GridCoordinate coordinate)
        {
            return grid.InBounds(coordinate) && (grid[coordinate] == Tile.Sand || grid[coordinate] == Tile.WetSand);
        }

        bool IsFloor(GridCoordinate coordinate)
        {
            return grid.InBounds(coordinate) && (grid[coordinate] == Tile.Clay || grid[coordinate] == Tile.Water);
        }
    }

    private static void PrintToFile(string fileName, Grid<Tile> grid)
    {
        grid.PrintToFile(fileName, tile => tile switch
        {
            Tile.Sand => '.',
            Tile.Clay => '#',
            Tile.WetSand => '|',
            Tile.Water => '~',
            _ => throw new NotImplementedException()
        });
    }

    [GeneratedRegex(@"(x|y)=(\d+), (x|y)=(\d+)..(\d+)")]
    private static partial Regex ButtonInputLineRegex();
}