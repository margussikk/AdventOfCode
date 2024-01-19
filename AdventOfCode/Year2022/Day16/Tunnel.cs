namespace AdventOfCode.Year2022.Day16;

internal class Tunnel(Valve leadsTo, int distance)
{
    public Valve LeadsTo { get; } = leadsTo;

    public int Distance { get; } = distance;
}
