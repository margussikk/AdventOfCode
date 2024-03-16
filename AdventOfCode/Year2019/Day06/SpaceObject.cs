namespace AdventOfCode.Year2019.Day06;

internal class SpaceObject(string name)
{
    public string Name { get; } = name;

    public SpaceObject? Orbits { get; set; } = null;

    public List<SpaceObject> OrbitedBy { get; } = [];

    public int CountOrbits(int level)
    {
        var totalLevel = level;

        foreach (var obj in OrbitedBy)
        {
            totalLevel += obj.CountOrbits(level + 1);
        }

        return totalLevel;
    }
}
