namespace AdventOfCode.Year2016.Day08;
internal class RotateColumnInstruction : Instruction
{
    public int Column { get; }
    public int Shift { get; }

    public RotateColumnInstruction(int column, int shift)
    {
        Column = column;
        Shift = shift;
    }
}
