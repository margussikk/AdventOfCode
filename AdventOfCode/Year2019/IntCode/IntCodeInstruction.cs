namespace AdventOfCode.Year2019.IntCode;

internal class IntCodeInstruction
{
    public IntCodeOpCode OpCode { get; set; }

    public int Length { get; set; }

    public List<int> Addresses { get; set; } = [];
}
