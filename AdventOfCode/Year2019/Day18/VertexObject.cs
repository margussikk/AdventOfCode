namespace AdventOfCode.Year2019.Day18;

internal class VertexObject
{
    public string Name { get; }
    public Tile Tile { get; }
    public int KeyBitMask { get; }

    public VertexObject(string name, Tile tile)
    {
        Name = name;
        Tile = tile;

        KeyBitMask = tile switch
        {
            Tile.Key => 1 << (name[0] - 'a'),
            Tile.Door => 1 << (name[0] - 'A'),
            _ => 0
        };
    }
}
