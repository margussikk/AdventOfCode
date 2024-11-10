namespace AdventOfCode.Year2022.Day16;

internal class Tunnel
{
    public Valve LeadsTo { get; }

    public int Distance { get; }

    public Tunnel(Valve leadsTo, int distance)
    {
        LeadsTo = leadsTo;
        Distance = distance;
    }
}
