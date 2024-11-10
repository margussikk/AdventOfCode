namespace AdventOfCode.Year2020.Day07;

internal class Containment
{
    public int Amount { get; }

    public Bag Bag { get; }

    public Containment(int amount, Bag bag)
    {
        Amount = amount;
        Bag = bag;
    }
}
