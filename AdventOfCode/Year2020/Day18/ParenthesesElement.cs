namespace AdventOfCode.Year2020.Day18;

internal class ParenthesesElement(bool open) : IElement
{
    public bool Open { get; } = open;

    public override string ToString()
    {
        if (Open)
        {
            return "(";
        }
        else
        {
            return ")";
        }
    }
}
