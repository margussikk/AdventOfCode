namespace AdventOfCode.Year2019.Day14;

internal class Chemical
{
    public int Quantity { get; }

    public string Name { get; }

    public Chemical(int quantity, string name)
    {
        Quantity = quantity;
        Name = name;
    }
}
