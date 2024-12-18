using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day18;

[Puzzle(2018, 18, "Settlers of The North Pole")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(character => character switch
        {
            '.' => Tile.Open,
            '|' => Tile.Trees,
            '#' => Tile.Lumberyard,
            _ => throw new InvalidOperationException()
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var grid = _grid.Clone();

        for (var minute = 0; minute < 10; minute++)
        {
            ApplyStrangeMagic(grid);
        }

        var answer = GetResourceValue(grid);

        return new PuzzleAnswer(answer, 535522);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const int totalCycles = 1_000_000_000;
        var answer = 0;
        var strangeMagicCycles = new Dictionary<int, int>();
        var totalResourceValues = new List<int>();

        var grid = _grid.Clone();

        for (var cycle = 0; cycle < int.MaxValue; cycle++)
        {
            var hashCode = grid.GetHashCode();
            if (strangeMagicCycles.TryGetValue(hashCode, out var cycleStart))
            {
                var cycleLength = cycle - cycleStart;
                var reminder = (totalCycles - cycle) % cycleLength;

                answer = totalResourceValues[cycleStart + reminder];
                break;
            }

            strangeMagicCycles[hashCode] = cycle;

            totalResourceValues.Add(GetResourceValue(grid));

            ApplyStrangeMagic(grid);
        }

        return new PuzzleAnswer(answer, 210160);
    }

    private static int GetResourceValue(Grid<Tile> grid)
    {
        var wooded = grid.Count(c => c.Object == Tile.Trees);
        var lumberyard = grid.Count(c => c.Object == Tile.Lumberyard);

        return wooded * lumberyard;
    }

    private static void ApplyStrangeMagic(Grid<Tile> grid)
    {
        var tileChanges = new List<TileChange>();

        // Find tile changes
        foreach (var cell in grid)
        {
            var nextTile = GetNextTile(grid, cell);
            if (nextTile != cell.Object)
            {
                tileChanges.Add(new TileChange(cell.Coordinate, nextTile));
            }
        }

        // Apply tile changes
        foreach (var tileChange in tileChanges)
        {
            grid[tileChange.Coordinate] = tileChange.Tile;
        }
    }

    private static Tile GetNextTile(Grid<Tile> grid, GridCell<Tile> gridCell)
    {
        if (gridCell.Object == Tile.Open)
        {
            var treeCount = grid.AroundNeighbors(gridCell.Coordinate).Count(c => c.Object == Tile.Trees);
            if (treeCount >= 3)
            {
                return Tile.Trees;
            }
        }
        else if (gridCell.Object == Tile.Trees)
        {
            var lumberyardCount = grid.AroundNeighbors(gridCell.Coordinate).Count(c => c.Object == Tile.Lumberyard);
            if (lumberyardCount >= 3)
            {
                return Tile.Lumberyard;
            }
        }
        else if (gridCell.Object == Tile.Lumberyard)
        {
            var adjacentToLumberyard = grid.AroundNeighbors(gridCell.Coordinate).Any(c => c.Object == Tile.Lumberyard);
            var adjacentToTrees = grid.AroundNeighbors(gridCell.Coordinate).Any(c => c.Object == Tile.Trees);

            if (!adjacentToLumberyard || !adjacentToTrees)
            {
                return Tile.Open;
            }
        }
        else
        {
            throw new InvalidOperationException();
        }

        return gridCell.Object;
    }

    private sealed record TileChange(GridCoordinate Coordinate, Tile Tile);
}