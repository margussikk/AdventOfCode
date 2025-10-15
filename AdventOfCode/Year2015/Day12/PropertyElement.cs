namespace AdventOfCode.Year2015.Day12;

internal class PropertyElement : Element
{
    public string Name { get; set; } = null!;
    public Element Element { get; set; } = null!;

    public override int SumOfNumbers(bool notRed)
    {
        return Element.SumOfNumbers(notRed);
    }
}
