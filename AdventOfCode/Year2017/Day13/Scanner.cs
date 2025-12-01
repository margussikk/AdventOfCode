namespace AdventOfCode.Year2017.Day13;

internal class Scanner
{
    private readonly int _period = 0;

    public int Depth { get; private set; }

    public int Range { get; private set; }

    public int Severity => Depth * Range;

    public Scanner(int depth, int range)
    {
        Depth = depth;
        Range = range;

        _period = (Range - 1) * 2;
    }

    public bool GetsCaught(int delay)
    {
        return (Depth + delay) % _period == 0;
    }
}
