namespace AdventOfCode.Year2020.Day18;

internal class OperatorElement(OperatorType operatorType) : IElement
{
    public OperatorType OperatorType { get; } = operatorType;

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
        if (OperatorType == OperatorType.Addition)
        {
            return "+";
        }
        else if (OperatorType == OperatorType.Multiplication)
        {
            return "*";
        }
        else
        {
            return string.Empty;
        }
    }
}
