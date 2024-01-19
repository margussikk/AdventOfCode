namespace AdventOfCode.Year2022.Day16;

internal class Valve(int id, string name, int flowRate)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public int FlowRate { get; } = flowRate;

    public List<Tunnel> Tunnels { get; } = [];

    public int OpenBitmask { get; } = 1 << id;
}
