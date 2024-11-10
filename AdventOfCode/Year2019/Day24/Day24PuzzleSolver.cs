using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day24;

[Puzzle(2019, 24, "Planet of Discord")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private BitGrid _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToBitGrid(character => character == '#');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = _grid.Clone();

        var biodiversityRatings = new HashSet<long>();

        long answer;

        while (true)
        {
            var biodiversityRating = CalculateBiodiversityRating(grid);
            if (biodiversityRatings.Add(biodiversityRating))
            {
                var gridUpdates = new List<GridCell<bool>>();

                foreach (var cell in grid)
                {
                    var bugCount = grid.SideNeighbors(cell.Coordinate).Count(x => x.Object);

                    if (cell.Object)
                    {
                        // A bug dies (becoming an empty space) unless there is exactly one bug adjacent to it.
                        if (bugCount != 1)
                        {
                            gridUpdates.Add(new GridCell<bool>(cell.Coordinate, false));
                        }
                    }
                    else
                    {
                        // An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
                        if (bugCount is 1 or 2)
                        {
                            gridUpdates.Add(new GridCell<bool>(cell.Coordinate, true));
                        }
                    }
                }

                foreach (var update in gridUpdates)
                {
                    grid[update.Coordinate] = update.Object;
                }
            }
            else
            {
                answer = biodiversityRating;
                break;
            }
        }

        return new PuzzleAnswer(answer, 27562081L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var centerCoordinate = new GridCoordinate(2, 2);

        var grids = new BitGrid[410];
        for (var i = 0; i < grids.Length; i++)
        {
            grids[i] = new BitGrid(_grid.Height, _grid.Width);
        }

        var zeroDepth = grids.Length / 2;

        grids[zeroDepth] = _grid.Clone();

        for (var minutes = 1; minutes <= 200; minutes++)
        {
            var gridCellUpdates = new List<GridCellUpdate>();

            for(var depth = zeroDepth - minutes; depth <= zeroDepth + minutes; depth++)
            {
                var grid = grids[depth];

                foreach(var cell in grid.Where(cell => cell.Coordinate != centerCoordinate))
                {
                    var bugCount = 0;

                    foreach(var neighborCoordinate in cell.Coordinate.SideNeighbors())
                    {
                        if (neighborCoordinate == centerCoordinate)
                        {
                            var innerGrid = grids[depth + 1];

                            var direction = cell.Coordinate.DirectionToward(neighborCoordinate);

                            bugCount += direction switch
                            {
                                GridDirection.Down => innerGrid.Row(0).Count(x => x.Object),
                                GridDirection.Up => innerGrid.Row(innerGrid.LastRowIndex).Count(x => x.Object),
                                GridDirection.Right => innerGrid.Column(0).Count(x => x.Object),
                                GridDirection.Left => innerGrid.Column(innerGrid.LastColumnIndex).Count(x => x.Object),
                                _ => throw new InvalidOperationException($"Invalid direction {direction}")
                            };
                        }
                        else if (grid.InBounds(neighborCoordinate))
                        {
                            if (grid[neighborCoordinate])
                            {
                                bugCount++;
                            }
                        }
                        else
                        {
                            var outerGrid = grids[depth - 1];

                            var direction = cell.Coordinate.DirectionToward(neighborCoordinate);
                            var outerCoordinate = centerCoordinate.Move(direction);

                            if (outerGrid[outerCoordinate])
                            {
                                bugCount++;
                            }
                        }
                    }


                    if (cell.Object)
                    {
                        // A bug dies (becoming an empty space) unless there is exactly one bug adjacent to it.
                        if (bugCount != 1)
                        {
                            gridCellUpdates.Add(new GridCellUpdate(depth, cell.Coordinate, false));
                        }
                    }
                    else
                    {
                        // An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
                        if (bugCount is 1 or 2)
                        {
                            gridCellUpdates.Add(new GridCellUpdate(depth, cell.Coordinate, true));
                        }
                    }
                }
            }

            // Update grid cell values
            foreach(var gridCellUpdate in gridCellUpdates)
            {
                grids[gridCellUpdate.Depth][gridCellUpdate.Coordinate] = gridCellUpdate.Value;
            }
        }

        var answer = grids.Sum(g => g.Count(cell => cell.Object));

        return new PuzzleAnswer(answer, 1893);
    }

    private static long CalculateBiodiversityRating(BitGrid grid)
    {
        return grid.Where(x => x.Object)
                   .Select(cell => cell.Coordinate.Row * grid.Width + cell.Coordinate.Column)
                   .Aggregate(0L, (current, index) => current | 1L << index);
    }

    private sealed record GridCellUpdate(int Depth, GridCoordinate Coordinate, bool Value);
}