namespace AdventOfCode.Year2016.Day11;
internal class Item
{
    public string Element { get; set; } = null!;
    public ItemType Type { get; set; }
    public int Floor { get; set; }

    public Item Clone()
    {
        return new Item
        {
            Element = Element,
            Type = Type,
            Floor = Floor
        };
    }

    public override string ToString()
    {
        return $"{Element} {Type.ToString().ToLower()} (floor {Floor})";
    }
}
