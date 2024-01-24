namespace AdventOfCode.Utilities.Geometry;

internal class LineSegment2D
{
    public Coordinate2D Start { get; }
    public Coordinate2D End { get; set; }

    public LineSegment2D(int x1, int y1, int x2, int y2)
    {
        Start = new Coordinate2D(x1, y1);
        End = new Coordinate2D(x2, y2);
    }

    public LineSegment2D(Coordinate2D startCoordinate, Coordinate2D endCoordinate)
    {
        Start = startCoordinate;
        End = endCoordinate;
    }
}
