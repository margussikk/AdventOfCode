namespace AdventOfCode.Year2019.Day06;

internal class SpaceObject(string name)
{
    public string Name { get; } = name;

    public SpaceObject? Orbits { get; set; }

    public List<SpaceObject> OrbitedBy { get; } = [];

    public int CountOrbits(int level)
    {
        return level + OrbitedBy.Sum(obj => obj.CountOrbits(level + 1));
    }
}
