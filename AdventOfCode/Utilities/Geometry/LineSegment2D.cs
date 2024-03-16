using AdventOfCode.Utilities.Numerics;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode.Utilities.Geometry;

internal class LineSegment2D : IEnumerable<Coordinate2D>
{
    public Coordinate2D Start { get; }
    public Coordinate2D End { get; }

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

    public bool Contains(Coordinate2D coordinate)
    {
        return IsOnSegment(Start, coordinate, End);
    }

    public IEnumerator<Coordinate2D> GetEnumerator()
    {
        var dx = Math.Sign(End.X - Start.X);
        var dy = Math.Sign(End.Y - Start.Y);

        if (dx == 0 && dy == 0)
        {
            yield return new Coordinate2D(Start.X, Start.Y);
        }
        else if (dx == 0 || dy == 0 || Math.Abs(dx) == Math.Abs(dy))
        {
            for (long x = Start.X, y = Start.Y; x <= End.X && y <= End.Y; x += dx, y += dy)
            {
                yield return new Coordinate2D(x, y);
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool TryFindOverlap(LineSegment2D other, [MaybeNullWhen(false)] out LineSegment2D overlapLineSegment)
    {
        // Find the four orientations needed for general and special cases 
        int orientation1 = FindOrientation(Start, End, other.Start);
        int orientation2 = FindOrientation(Start, End, other.End);
        int orientation3 = FindOrientation(other.Start, other.End, Start);
        int orientation4 = FindOrientation(other.Start, other.End, End);

        // General case 
        if (orientation1 != orientation2 && orientation3 != orientation4)
        {
            var vector = new Vector2D(End.X - Start.X, End.Y - Start.Y);
            var otherVector = new Vector2D(other.End.X - other.Start.X, other.End.Y - other.Start.Y);

            var determinant = vector.DX * otherVector.DY - otherVector.DX * vector.DY;

            var t = Convert.ToDouble((Start.Y - other.Start.Y) * otherVector.DX - (Start.X - other.Start.X) * otherVector.DY) / determinant;
            if (t >= 0 && t <= 1)
            {
                var intersectionCoordinate = Start + vector * t;
                overlapLineSegment = new LineSegment2D(intersectionCoordinate, intersectionCoordinate);
                return true;
            }

            var u = Convert.ToDouble((Start.X - other.Start.X) * vector.DY - (End.Y - other.Start.Y) * vector.DX) / determinant;
            if (u >= 0 && u <= 1)
            {
                var intersectionCoordinate = other.Start + vector * u;
                overlapLineSegment = new LineSegment2D(intersectionCoordinate, intersectionCoordinate);
                return true;
            }

            overlapLineSegment = default;
            return false;
        }
        // Special Cases 
        else if ((orientation1 == 0 && IsOnSegment(Start, other.Start, End)) ||
                 (orientation2 == 0 && IsOnSegment(Start, other.End, End)) ||
                 (orientation3 == 0 && IsOnSegment(other.Start, Start, other.End)) ||
                 (orientation4 == 0 && IsOnSegment(other.Start, End, other.End)))
        {
            Coordinate2D[] coordinates = [Start, End, other.Start, other.End];
            var orderedCoordinates = coordinates.OrderBy(c => c.X)
                                                .ThenBy(c => c.Y)
                                                .ToList();

            overlapLineSegment = new LineSegment2D(orderedCoordinates[1], orderedCoordinates[2]);
            return true;
        }
        else
        {
            overlapLineSegment = default;
            return false;
        }
    }

    // Given three collinear points, function checks if point lies on line segment
    private static bool IsOnSegment(Coordinate2D startCoordinate, Coordinate2D coordinate, Coordinate2D endCoordinate)
    {
        return coordinate.X <= Math.Max(startCoordinate.X, endCoordinate.X) &&
               coordinate.X >= Math.Min(startCoordinate.X, endCoordinate.X) &&
               coordinate.Y <= Math.Max(startCoordinate.Y, endCoordinate.Y) &&
               coordinate.Y >= Math.Min(startCoordinate.Y, endCoordinate.Y);
    }

    // To find orientation of ordered triplet. 
    // The function returns following values 
    // 0 --> Collinear 
    // 1 --> Clockwise 
    // 2 --> Counter clockwise 
    private static int FindOrientation(Coordinate2D coordinate1, Coordinate2D coordinate2, Coordinate2D coordinate3)
    {
        // See https://www.geeksforgeeks.org/orientation-3-ordered-points/ 
        // for details of below formula. 

        var val = (coordinate2.Y - coordinate1.Y) * (coordinate3.X - coordinate2.X) -
                  (coordinate2.X - coordinate1.X) * (coordinate3.Y - coordinate2.Y);

        return val switch
        {
            0 => 0, // Collinear
            > 0 => 1, // Clockwise
            _ => 2, // Counter clockwise 
        };
    }
}
