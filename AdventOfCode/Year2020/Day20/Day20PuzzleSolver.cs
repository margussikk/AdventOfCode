using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2020.Day20;

[Puzzle(2020, 20, "Jurassic Jigsaw")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private List<Tile> _tiles = [];
    private static readonly string[] _seaMonsterLines =
    [
        "                  # ",
        "#    ##    ##    ###",
        " #  #  #  #  #  #   "
    ];

    public void ParseInput(string[] inputLines)
    {
        _tiles = inputLines
            .SelectToChunks()
            .Select(Tile.Parse)
            .ToList();
    }

    // The Solution assumes that every border bitmask (and its reversal) exists only once. We care only about corned tiles, no need to rearrange the tiles
    public PuzzleAnswer GetPartOneAnswer()
    {
        var cornerTiles = new List<Tile>();

        foreach (var tile in _tiles)
        {
            var neighborsCount = 0;

            foreach (var neighborTile in _tiles.Where(t => t.Id != tile.Id))
            {
                for (var orientation = 0; orientation < 8; orientation++)
                {
                    for (var border = TileBorder.Top; border <= TileBorder.Left; border++)
                    {
                        if (tile.BorderBitmasks[0, border] == neighborTile.BorderBitmasks[orientation, (border + 2) % 4])
                        {
                            neighborsCount++;
                        }
                    }
                }
            }

            if (neighborsCount == 2)
            {
                cornerTiles.Add(tile);
            }
        }

        if (cornerTiles.Count != 4)
        {
            throw new InvalidOperationException("Couldn't find 4 corner tiles");
        }

        var answer = cornerTiles.Aggregate(1L, (acc, tile) => acc * tile.Id);

        return new PuzzleAnswer(answer, 17032646100079L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Find tile arrangements: coordinate, orientation
        var tileArrangementsDict = new Dictionary<int, TileArrangement>();

        var queue = new Queue<TileArrangement>();

        var currentTileArrangement = new TileArrangement(_tiles[0], new GridCoordinate(0, 0), 0);

        queue.Enqueue(currentTileArrangement);
        while (queue.TryDequeue(out currentTileArrangement))
        {
            if (tileArrangementsDict.ContainsKey(currentTileArrangement.Tile.Id))
            {
                continue;
            }

            tileArrangementsDict[currentTileArrangement.Tile.Id] = new TileArrangement(currentTileArrangement.Tile, currentTileArrangement.Coordinate, currentTileArrangement.Orientation);

            foreach (var neighborTile in _tiles.Where(t => !tileArrangementsDict.ContainsKey(t.Id)))
            {
                for (var orientation = 0; orientation < 8; orientation++)
                {
                    for (var border = TileBorder.Top; border <= TileBorder.Left; border++)
                    {
                        if (currentTileArrangement.Tile.BorderBitmasks[currentTileArrangement.Orientation, border] !=
                            neighborTile.BorderBitmasks[orientation, (border + 2) % 4]) continue;

                        var coordinate = border switch
                        {
                            TileBorder.Top => currentTileArrangement.Coordinate.Move(GridDirection.Up),
                            TileBorder.Right => currentTileArrangement.Coordinate.Move(GridDirection.Right),
                            TileBorder.Bottom => currentTileArrangement.Coordinate.Move(GridDirection.Down),
                            TileBorder.Left => currentTileArrangement.Coordinate.Move(GridDirection.Left),
                            _ => throw new InvalidOperationException("Invalid tile border")
                        };

                        var newTileArrangement = new TileArrangement(neighborTile, coordinate, orientation);
                        queue.Enqueue(newTileArrangement);
                    }
                }
            }
        }

        // Build large image
        var tileArrangementsGrid = CreateTileArrangementsGrid(tileArrangementsDict.Values);

        var smallerTileSize = _tiles[0].Image.Height - 2;

        var largeImage = new BitGrid(tileArrangementsGrid.Height * smallerTileSize, tileArrangementsGrid.Width * smallerTileSize);
        foreach (var tileCell in tileArrangementsGrid)
        {
            var orientedTileImage = tileCell.Object.Tile.GetOrientedImage(tileCell.Object.Orientation);

            var largeImageRow = tileCell.Coordinate.Row * smallerTileSize - 1;  // - 1 to avoid gaps
            var largeImageColumn = tileCell.Coordinate.Column * smallerTileSize - 1;  // - 1 to avoid gaps

            foreach (var tileImageCell in orientedTileImage)
            {
                if (tileImageCell.Coordinate.Row > 0 && tileImageCell.Coordinate.Row < orientedTileImage.LastRowIndex &&
                    tileImageCell.Coordinate.Column > 0 && tileImageCell.Coordinate.Column < orientedTileImage.LastColumnIndex)
                {
                    largeImage[largeImageRow + tileImageCell.Coordinate.Row, largeImageColumn + tileImageCell.Coordinate.Column] = tileImageCell.Object;
                }
            }
        }

        // Find sea monster
        var seaMonsterCoordinates = new HashSet<GridCoordinate>();

        var seaMonsterPattern = _seaMonsterLines.SelectToBitGrid(character => character == '#');

        for (var orientation = 0; orientation < 8; orientation++)
        {
            for (var row = 0; row < largeImage.Height - seaMonsterPattern.Height; row++)
            {
                for (var column = 0; column < largeImage.Width - seaMonsterPattern.Width; column++)
                {
                    var found = seaMonsterPattern.All(cell => !cell.Object || largeImage[cell.Coordinate.Row + row, cell.Coordinate.Column + column]);
                    if (!found) continue;

                    var coordinates = seaMonsterPattern
                        .Where(c => c.Object)
                        .Select(c => new GridCoordinate(c.Coordinate.Row + row, c.Coordinate.Column + column));

                    seaMonsterCoordinates.UnionWith(coordinates);
                }
            }

            // Instead of rotating the large image, rotate the smaller sea monster pattern, it should take fewer operations
            if (orientation == 3)
            {
                seaMonsterPattern = seaMonsterPattern
                    .RotateClockwise()
                    .FlipHorizontally();
            }
            else
            {
                seaMonsterPattern = seaMonsterPattern.RotateClockwise();
            }
        }

        var answer = largeImage.Count(c => c.Object) - seaMonsterCoordinates.Count;

        return new PuzzleAnswer(answer, 2006);
    }

    private static Grid<TileArrangement> CreateTileArrangementsGrid(IEnumerable<TileArrangement> tileArrangements)
    {
        var minRow = 0;
        var maxRow = 0;
        var minColumn = 0;
        var maxColumn = 0;

        foreach (var tileArrangement in tileArrangements)
        {
            minRow = int.Min(tileArrangement.Coordinate.Row, minRow);
            maxRow = int.Max(tileArrangement.Coordinate.Row, maxRow);

            minColumn = int.Min(tileArrangement.Coordinate.Column, minColumn);
            maxColumn = int.Max(tileArrangement.Coordinate.Column, maxColumn);
        }

        var tileArrangementsGrid = new Grid<TileArrangement>(maxRow - minRow + 1, maxColumn - minColumn + 1);
        foreach (var tileArrangement in tileArrangements)
        {
            tileArrangementsGrid[tileArrangement.Coordinate.Row - minRow, tileArrangement.Coordinate.Column - minColumn] = tileArrangement;
        }

        return tileArrangementsGrid;
    }
}