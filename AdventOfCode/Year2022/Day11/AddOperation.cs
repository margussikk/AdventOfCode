namespace AdventOfCode.Year2022.Day11;

internal class AddOperation : Operation
{
    public AddOperation(IOperand operand) : base(operand) { }

    public override long CalculateWorryLevel(long oldWorryLevel)
    {
        if (Operand is NumberOperand operandNumber)
        {
            return oldWorryLevel + operandNumber.Value;
        }

        return oldWorryLevel + oldWorryLevel;
    }
}
