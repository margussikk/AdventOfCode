using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day15;

internal class Sensor
{
    public Coordinate2D Coordinate { get; }

    public Coordinate2D BeaconCoordinate { get; }

    public Coordinate2D TopCoordinate { get; }

    public Coordinate2D BottomCoordinate { get; }

    public Coordinate2D LeftCoordinate { get; }

    public Coordinate2D RightCoordinate { get; }

    private readonly long _manhattanDistance;

    public Sensor(Coordinate2D coordinate, Coordinate2D beaconCoordinate)
    {
        Coordinate = coordinate;
        BeaconCoordinate = beaconCoordinate;

        _manhattanDistance = Coordinate.ManhattanDistanceBetween(BeaconCoordinate);

        TopCoordinate = new Coordinate2D(Coordinate.X, Coordinate.Y - _manhattanDistance);
        BottomCoordinate = new Coordinate2D(Coordinate.X, Coordinate.Y + _manhattanDistance);
        LeftCoordinate = new Coordinate2D(Coordinate.X - _manhattanDistance, Coordinate.Y);
        RightCoordinate = new Coordinate2D(Coordinate.X + _manhattanDistance, Coordinate.Y);
    }

    public long GetAdjustedLeftX(long y)
    {
        var deviationX = Math.Abs(Coordinate.Y - y);
        var adjustedLeftX = LeftCoordinate.X + deviationX;

        return adjustedLeftX;
    }

    public long GetAdjustedRightX(long y)
    {
        var deviationX = Math.Abs(Coordinate.Y - y);
        var adjustedRightX = RightCoordinate.X - deviationX;

        return adjustedRightX;
    }

    public bool Detectable(Coordinate2D location)
    {
        return Coordinate.ManhattanDistanceBetween(location) <= _manhattanDistance;
    }
}
