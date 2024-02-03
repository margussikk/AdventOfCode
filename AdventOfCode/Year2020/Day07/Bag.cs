namespace AdventOfCode.Year2020.Day07;

internal class Bag(string color)
{
    public string Color { get; private set; } = color;

    public List<Bag> Containers { get; } = [];

    public List<Containment> Containments { get; } = [];

    public void AddContainer(Bag bag)
    {
        Containers.Add(bag);
    }

    public void AddContainment(Containment containment)
    {
        Containments.Add(containment);
    }
}
