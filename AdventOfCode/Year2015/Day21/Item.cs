namespace AdventOfCode.Year2015.Day21;

internal class Item
{
    public string Name { get; }
    public int Cost { get; }
    public int Damage { get; }
    public int Armor { get; }

    public Item(string name, int cost, int damage, int armor)
    {
        Name = name;
        Cost = cost;
        Damage = damage;
        Armor = armor;
    }
}
