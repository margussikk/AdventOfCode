namespace AdventOfCode.Year2022.Day11;

internal abstract class Operation(Operand operand)
{
    public Operand Operand { get; set; } = operand;

    public abstract long CalculateWorryLevel(long oldWorryLevel);
}
