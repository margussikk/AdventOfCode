namespace AdventOfCode.Utilities.Geometry;
internal interface IGrid<T> : IEnumerable<GridCell<T>>
{
    int Width { get; }

    int Height { get; }

    int LastRowIndex { get; }

    int LastColumnIndex { get; }

    T this[GridCoordinate coordinate] { get; set; }

    GridCell<T> Cell(GridCoordinate coordinate);

    bool InBounds(GridCoordinate coordinate);

    IEnumerable<GridCell<T>> SideNeighbors(GridCoordinate coordinate, GridDirection direction = GridDirection.AllSides);
}
