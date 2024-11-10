namespace AdventOfCode.Year2019.IntCode;

internal class IntCodeInstruction
{
    public IntCodeOpCode OpCode { get; init; }

    public int Length { get; init; }

    public List<int> Addresses { get; } = [];
}
