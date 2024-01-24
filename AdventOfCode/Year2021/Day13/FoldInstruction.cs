namespace AdventOfCode.Year2021.Day13;

internal class FoldInstruction(Axis axis, int foldLine)
{
    public Axis Axis { get; } = axis;
    public int FoldLine { get; } = foldLine;
}
