namespace AdventOfCode.Year2015.Day13;

internal class Guest
{
    public string Name { get; }
    public List<NeighborHappiness> Happinesses { get; } = [];

    public Guest(string name)
    {
        Name = name;
    }

    public Guest Clone()
    {
        var guest = new Guest(Name);
        guest.Happinesses.AddRange(Happinesses);

        return guest;
    }
}
