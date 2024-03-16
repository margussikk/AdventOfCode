namespace AdventOfCode.Year2019.Day18;

internal class VertexObject
{
    public string Name { get; }
    public Tile Tile { get; }
    public int KeyBitMask {  get; }

    public VertexObject(string name, Tile tile)
    {
        Name = name;
        Tile = tile;

        if (tile == Tile.Key)
        {
            KeyBitMask = 1 << (name[0] - 'a');
        }
        else if (tile == Tile.Door)
        {
            KeyBitMask = 1 << (name[0] - 'A');
        }
        else
        {
            KeyBitMask = 0;
        }
    }
}
