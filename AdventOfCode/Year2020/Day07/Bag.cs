namespace AdventOfCode.Year2020.Day07;

internal class Bag
{
    public string Color { get; }

    public List<Bag> Containers { get; } = [];

    public List<Containment> Containments { get; } = [];

    public Bag(string color)
    {
        Color = color;
    }

    public void AddContainer(Bag bag)
    {
        Containers.Add(bag);
    }

    public void AddContainment(Containment containment)
    {
        Containments.Add(containment);
    }
}
