namespace AdventOfCode.Utilities.Geometry;

internal static class MeasurementFunctions
{
    // https://en.wikipedia.org/wiki/Shoelace_formula
    // Area = |x1y2 - y1x2 + x2y3 - y2x3 + ... + xny1 - ynx1| / 2
    public static long ShoelaceFormula(IReadOnlyList<GridCoordinate> coordinates)
    {
        var sum = 0L;

        for (var index = 0; index < coordinates.Count; index++)
        {
            var index2 = (index + 1) % coordinates.Count;

            sum += Convert.ToInt64(coordinates[index].Row) * (long)coordinates[index2].Column -
                   (long)coordinates[index].Column * (long)coordinates[index2].Row;

        }

        return Math.Abs(sum) / 2; // area
    }

    // Pick's theorem (https://en.wikipedia.org/wiki/Pick%27s_theorem)
    // Area = [interior points] + [boundary points] / 2 - 1
    // [interior points] = Area - [boundary points] / 2 + 1
    public static long PicksTheoremGetInteriorPoints(long area, long boundaryPoints)
    {
        return area - boundaryPoints / 2 + 1;
    }

    public static int ManhattanDistance(GridCoordinate coordinate1, GridCoordinate coordinate2)
    {
        return Math.Abs(coordinate2.Row - coordinate1.Row) +
               Math.Abs(coordinate2.Column - coordinate1.Column);
    }

    public static long ManhattanDistance(Coordinate2D coordinate1, Coordinate2D coordinate2)
    {
        return Math.Abs(coordinate2.X - coordinate1.X) +
               Math.Abs(coordinate2.Y - coordinate1.Y);
    }

    public static long ManhattanDistance(Coordinate3D coordinate1, Coordinate3D coordinate2)
    {
        return Math.Abs(coordinate2.X - coordinate1.X) +
               Math.Abs(coordinate2.Y - coordinate1.Y) +
               Math.Abs(coordinate2.Z - coordinate1.Z);
    }

    public static int ManhattanDistanceLoop(IReadOnlyList<GridCoordinate> coordinates)
    {
        var distance = 0;

        for (var index = 0; index < coordinates.Count; index++)
        {
            var index2 = (index + 1) % coordinates.Count;

            distance += ManhattanDistance(coordinates[index], coordinates[index2]);
        }

        return distance;
    }
}
