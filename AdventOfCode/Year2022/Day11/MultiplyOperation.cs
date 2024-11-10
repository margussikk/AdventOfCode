namespace AdventOfCode.Year2022.Day11;

internal class MultiplyOperation : Operation
{
    public MultiplyOperation(IOperand operand) : base(operand) { }

    public override long CalculateWorryLevel(long oldWorryLevel)
    {
        if (Operand is NumberOperand operandNumber)
        {
            return oldWorryLevel * operandNumber.Value;
        }

        return oldWorryLevel * oldWorryLevel;
    }
}
