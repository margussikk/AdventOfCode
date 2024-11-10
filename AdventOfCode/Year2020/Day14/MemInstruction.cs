using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020.Day14;

internal partial class MemInstruction: Instruction
{
    public long Address { get; private set; }

    public long Value { get; private set; }

    public new static MemInstruction Parse(string input)
    {
        var matches = InputRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse mem instruction");
        }

        var memInstruction = new MemInstruction
        {
            Address = long.Parse(matches[0].Groups[1].Value),
            Value = long.Parse(matches[0].Groups[2].Value)
        };

        return memInstruction;
    }

    [GeneratedRegex(@"mem\[(\d+)\] = (\d+)")]
    private static partial Regex InputRegex();
}
