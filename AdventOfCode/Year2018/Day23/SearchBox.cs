using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day23;

internal class SearchBox
{
    private readonly Region3D _region;

    public List<Nanobot> Nanobots { get; } = [];

    public long Volume => _region.Volume;

    public SearchBox(Region3D region)
    {
        _region = region;
    }

    public bool InBounds(Nanobot nanobot)
    {
        return _region.Distance(nanobot.Coordinate) <= nanobot.SignalRadius;
    }

    public IEnumerable<SearchBox> Divide()
    {
        return _region.Divide().Select(r => new SearchBox(r));
    }

    public SearchBoxRank GetRank()
    {
        var distance = _region.CenterCoordinate.ManhattanDistanceTo(Coordinate3D.Zero);
        return new SearchBoxRank(Nanobots.Count, distance, Volume);
    }
}
