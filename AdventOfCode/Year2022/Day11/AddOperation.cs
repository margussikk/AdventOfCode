namespace AdventOfCode.Year2022.Day11;

internal class AddOperation(Operand operand) : Operation(operand)
{
    public override long CalculateWorryLevel(long oldWorryLevel)
    {
        if (Operand is NumberOperand operandNumber)
        {
            return oldWorryLevel + operandNumber.Value;
        }
        else
        {
            return oldWorryLevel + oldWorryLevel;
        }
    }
}
