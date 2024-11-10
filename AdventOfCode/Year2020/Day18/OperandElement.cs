namespace AdventOfCode.Year2020.Day18;

internal class OperandElement : IElement
{
    public long Value { get; }

    public OperandElement(long value)
    {
        Value = value;
    }
    
    public override string ToString()
    {
        return $"{Value}";
    }
}
