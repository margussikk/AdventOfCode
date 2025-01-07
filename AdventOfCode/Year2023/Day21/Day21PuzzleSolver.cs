using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2023.Day21;

//
//                              +-----+-----+-----+
//                              |     |  0  |     |
//                              |     | 000 |     |
//                              |    1|00000|1    |
//                              |   11|00000|11   |
//                              |  111|00000|111  |
//                        +-----+-----+-----+-----+-----+
//                        |     |  222|33333|222  |     |
//                        |     | 2222|33333|2222 |     |
//                        |    1|22222|33333|22222|1    |           Solution works by filling every grid type to count how many garden spots are reachable in that grid.
//                        |   11|22222|33333|22222|11   |           Secondly amount of different grids and garden spots in them are used to calculate total reachable garden spots count.
//                        |  111|22222|33333|22222|111  |
//                        +-----+-----+-----+-----+-----+
//                        |  000|33333|44444|33333|000  |           0 = top, left, right, bottom
//                        | 0000|33333|44444|33333|0000 |           1 = smallTopLeft, smallTopRight, smallBottomLeft, smallBottomRight
//                        |00000|33333|44444|33333|00000|           2 = largeTopLeft, largeTopRight, largeBottomLeft, largeBottomRight
//                        | 0000|33333|44444|33333|0000 |           3 = odd
//                        |  000|33333|44444|33333|000  |           4 = even
//                        +-----+-----+-----+-----+-----+
//                        |  111|22222|33333|22222|111  |
//                        |   11|22222|33333|22222|11   |
//                        |    1|22222|33333|22222|1    |
//                        |     | 2222|33333|2222 |     |
//                        |     |  222|33333|222  |     |
//                        +-----+-----+-----+-----+-----+
//                              |  111|00000|111  |
//                              |   11|00000|11   |
//                              |    1|00000|1    |
//                              |     | 000 |     |
//                              |     |  0  |     |
//                              +-----+-----+-----+


[Puzzle(2023, 21, "Step Counter")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _tiles = new(0, 0);
    private GridCoordinate _startCoordinate = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _tiles = inputLines.SelectToGrid(character => character switch
        {
            'S' => Tile.StartingPosition,
            '.' => Tile.GardenPlot,
            '#' => Tile.Rock,
            _ => throw new InvalidOperationException()
        });

        _startCoordinate = _tiles.FindCoordinate(x => x == Tile.StartingPosition)
            ?? throw new InvalidOperationException("Start position not found");
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CountReachableGardenSpots(_startCoordinate.Row, _startCoordinate.Column, 64);

        return new PuzzleAnswer(answer, 3716);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        if (_startCoordinate.Row != 65 || _startCoordinate.Column != 65)
        {
            throw new InvalidOperationException("This solution expects Start to be at 65,65");
        }

        const long stepCount = 26501365L; // 202300 * 131 + 65
        var gridCount = (stepCount - 65) / _tiles.Height; // Assume grids are squares

        const int firstRow = 0;
        var lastRow = _tiles.LastRowIndex;
        const int firstColumn = 0;
        var lastColumn = _tiles.LastColumnIndex;
        var middleRow = _tiles.Height / 2;
        var middleColumn = _tiles.Width / 2;


        // Fill every grid type to count possible reached garden spots.
        // Some grids are filled fully, some partially using various starting points and step counts.
        // See diagram above.
        var even = CountReachableGardenSpots(middleRow, middleColumn, 129);
        var odd = CountReachableGardenSpots(middleRow, middleColumn, 130);

        var left = CountReachableGardenSpots(middleRow, lastColumn, 130);
        var right = CountReachableGardenSpots(middleRow, firstColumn, 130);
        var top = CountReachableGardenSpots(lastRow, middleColumn, 130);
        var bottom = CountReachableGardenSpots(firstRow, middleColumn, 130);

        var smallTopLeft = CountReachableGardenSpots(lastRow, lastColumn, 64);
        var smallTopRight = CountReachableGardenSpots(lastRow, firstColumn, 64);
        var smallBottomLeft = CountReachableGardenSpots(firstRow, lastColumn, 64);
        var smallBottomRight = CountReachableGardenSpots(firstRow, firstColumn, 64);

        var largeTopLeft = CountReachableGardenSpots(lastRow, lastColumn, 130 + 65);
        var largeTopRight = CountReachableGardenSpots(lastRow, firstColumn, 130 + 65);
        var largeBottomLeft = CountReachableGardenSpots(firstRow, lastColumn, 130 + 65);
        var largeBottomRight = CountReachableGardenSpots(firstRow, firstColumn, 130 + 65);

        var answer = (gridCount - 1) * (gridCount - 1) * even +
            gridCount * gridCount * odd +
            top + left + right + bottom +
            gridCount * (smallTopLeft + smallTopRight + smallBottomLeft + smallBottomRight) +
            (gridCount - 1) * (largeTopLeft + largeTopRight + largeBottomLeft + largeBottomRight);

        return new PuzzleAnswer(answer, 616583483179597L);
    }

    private long CountReachableGardenSpots(int startRow, int startColumn, int steps)
    {
        var visited = new HashSet<GridCoordinate>[]
        {
            [], // Even
            []  // Odd
        };
        var gardeners = new Queue<Gardener>();

        var startCoordinate = new GridCoordinate(startRow, startColumn);

        var gardener = new Gardener(startCoordinate, 0);
        gardeners.Enqueue(gardener);

        while (gardeners.TryDequeue(out gardener))
        {
            if (gardener.Steps > steps || visited[gardener.Steps % 2].Contains(gardener.Coordinate))
            {
                continue;
            }

            visited[gardener.Steps % 2].Add(gardener.Coordinate);

            foreach (var neighborCell in _tiles.SideNeighbors(gardener.Coordinate).Where(c => c.Object != Tile.Rock))
            {
                var newGardener = new Gardener(neighborCell.Coordinate, gardener.Steps + 1);

                gardeners.Enqueue(newGardener);
            }
        }

        return visited[steps % 2].Count;
    }
}
