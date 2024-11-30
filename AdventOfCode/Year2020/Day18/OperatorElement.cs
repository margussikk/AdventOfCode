namespace AdventOfCode.Year2020.Day18;

internal class OperatorElement : IElement
{
    public OperatorType OperatorType { get; }

    public OperatorElement(OperatorType operatorType)
    {
        OperatorType = operatorType;
    }

    public long Calculate(long value1, long value2)
    {
        return OperatorType switch
        {
            OperatorType.Addition => value1 + value2,
            OperatorType.Multiplication => value1 * value2,
            _ => throw new InvalidOperationException("Invalid operator type")
        };
    }

    public override string ToString()
    {
        return OperatorType switch
        {
            OperatorType.Addition => "+",
            OperatorType.Multiplication => "*",
            _ => string.Empty
        };
    }
}
