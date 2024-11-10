namespace AdventOfCode.Year2021.Day13;

internal class FoldInstruction
{
    public Axis Axis { get; }
    public int FoldLine { get; }

    public FoldInstruction(Axis axis, int foldLine)
    {
        Axis = axis;
        FoldLine = foldLine;
    }
}
