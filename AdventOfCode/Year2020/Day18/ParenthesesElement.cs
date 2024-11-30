namespace AdventOfCode.Year2020.Day18;

internal class ParenthesesElement : IElement
{
    public bool Open { get; }

    public ParenthesesElement(bool open)
    {
        Open = open;
    }

    public override string ToString()
    {
        return Open ? "(" : ")";
    }
}
