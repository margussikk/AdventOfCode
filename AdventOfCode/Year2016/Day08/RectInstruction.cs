namespace AdventOfCode.Year2016.Day08;
internal class RectInstruction : Instruction
{
    public int Width { get; }
    public int Height { get; }

    public RectInstruction(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
