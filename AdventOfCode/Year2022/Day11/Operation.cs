namespace AdventOfCode.Year2022.Day11;

internal abstract class Operation
{
    public IOperand Operand { get; }

    protected Operation(IOperand operand)
    {
        Operand = operand;
    }

    public abstract long CalculateWorryLevel(long oldWorryLevel);
}
