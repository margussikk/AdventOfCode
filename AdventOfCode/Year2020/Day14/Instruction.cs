namespace AdventOfCode.Year2020.Day14;

internal abstract class Instruction
{
    public static Instruction Parse(string input)
    {
        if (input.StartsWith("mask ="))
        {
            return MaskInstruction.Parse(input);
        }
        else if (input.StartsWith("mem["))
        {
            return MemInstruction.Parse(input);
        }
        else
        {
            throw new InvalidOperationException("Failed to parse instruction");
        }
    }
}
