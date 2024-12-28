using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day20;
internal abstract class RouteElement
{
    public List<RouteElement> Elements { get; private set; } = [];

    public abstract GridCoordinate Walk(GridCoordinate coordinate, InfiniteBitGrid grid);
}
