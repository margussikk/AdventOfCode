namespace AdventOfCode.Year2019.IntCode;

internal class IntCodeInstruction
{
    public IntCodeOpCode OpCode { get; set; }

    public long[] Arguments { get; set; } = [];
}
