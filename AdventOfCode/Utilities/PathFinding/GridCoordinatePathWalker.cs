using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Utilities.PathFinding;
internal class GridCoordinatePathWalker
{
    public GridCoordinate Coordinate { get; set; }
    public GridCoordinate? PreviousCoordinate { get; set; }

    public List<GridCoordinate> Path { get; set; } = [];

    public int Cost { get; set; }
}
