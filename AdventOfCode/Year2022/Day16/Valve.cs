namespace AdventOfCode.Year2022.Day16;

internal class Valve
{
    public int Id { get; }

    public string Name { get; }

    public int FlowRate { get; }

    public int OpenBitmask { get; }
    
    public List<Tunnel> Tunnels { get; } = [];

    public Valve(int id, string name, int flowRate)
    {
        Id = id;
        Name = name;
        FlowRate = flowRate;
        OpenBitmask = 1 << id;
    }
}
