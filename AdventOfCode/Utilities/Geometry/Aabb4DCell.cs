namespace AdventOfCode.Utilities.Geometry;

internal class Aabb4DCell(Coordinate4D coordinate, bool active)
{
    public Coordinate4D Coordinate { get; } = coordinate;
    public bool Active { get; } = active;
}
