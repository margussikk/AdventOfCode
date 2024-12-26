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

            sum += Convert.ToInt64(coordinates[index].Row) * coordinates[index2].Column -
                   Convert.ToInt64(coordinates[index].Column) * coordinates[index2].Row;

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

    public static int ManhattanDistanceLoop(IReadOnlyList<GridCoordinate> coordinates)
    {
        var distance = 0;

        for (var index = 0; index < coordinates.Count; index++)
        {
            var index2 = (index + 1) % coordinates.Count;

            distance += coordinates[index].ManhattanDistanceBetween(coordinates[index2]);
        }

        return distance;
    }
}
