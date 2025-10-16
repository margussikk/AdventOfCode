namespace AdventOfCode.Year2015.Day13;

internal class NeighborHappiness
{
    public string NeighborName { get; }
    public int Change { get; }

    public NeighborHappiness(string neighborName, int change)
    {
        NeighborName = neighborName;
        Change = change;
    }
}
