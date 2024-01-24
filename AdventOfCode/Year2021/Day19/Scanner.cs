using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2021.Day19;

internal class Scanner
{
    public int Id { get; }

    public Coordinate3D AbsoluteLocation { get; set; }

    public int Orientation { get; set; }

    public bool Aligned { get; set; }

    private readonly List<Coordinate3D>[] _beaconLocations;
    public IReadOnlyList<List<Coordinate3D>> BeaconLocations => _beaconLocations;

    public Scanner(int id)
    {
        Id = id;

        _beaconLocations = new List<Coordinate3D>[24];
        for (var i = 0; i < 24; i++)
        {
            _beaconLocations[i] = [];
        }
    }

    public Scanner Clone()
    {
        var clone = new Scanner(Id)
        {
            AbsoluteLocation = AbsoluteLocation,
            Orientation = Orientation,
            Aligned = Aligned,
        };

        foreach (var beaconLocation in BeaconLocations[0])
        {
            clone.AddBeaconMeasurement(beaconLocation);
        }
        
        return clone;
    }

    public void AddBeaconMeasurement(Coordinate3D measurement)
    {
        var orientations = measurement.Orientations();

        for (var i = 0; i < 24; i++)
        {
            _beaconLocations[i].Add(orientations[i]);
        }
    }
}
