using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2020.Day20;

internal record TileArrangement
{
    public Tile Tile { get; }
    public GridCoordinate Coordinate { get; }
    public int Orientation { get; }

    public TileArrangement(Tile tile, GridCoordinate coordinate, int orientation)
    {
        Tile = tile;
        Coordinate = coordinate;
        Orientation = orientation;
    }
}
