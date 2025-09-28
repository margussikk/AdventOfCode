namespace AdventOfCode.Year2016.Day08;
internal class RotateRowInstruction : Instruction
{
    public int Row { get; }
    public int Shift { get; }

    public RotateRowInstruction(int row, int shift)
    {
        Row = row;
        Shift = shift;
    }
}
