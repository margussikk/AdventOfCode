using AdventOfCode.Year2018.Common;

namespace AdventOfCode.Year2018.Day16;

internal class Sample
{
    public int[] BeforeRegisters { get; private set; } = new int[4];
    public int[] InstructionValues { get; private set; } = new int[4];
    public int[] AfterRegisters { get; private set; } = new int[4];

    public bool BehavesLikeOpcode(OpCode opCode)
    {
        var device = new Device(BeforeRegisters);

        var instruction = Instruction.Parse(InstructionValues);
        instruction.ChangeOpCode(opCode);

        if (!instruction.IsValid(4))
        {
            return false;
        }

        device.RunInstruction(instruction);
        return device.Registers.SequenceEqual(AfterRegisters);
    }

    public bool BehavesLikeThreeOpCodes()
    {
        return Enum.GetValues<OpCode>()
                   .Count(BehavesLikeOpcode) >= 3;
    }

    public static Sample Parse(string[] inputLines)
    {
        var sample = new Sample();

        // Before registers
        if (!inputLines[0].StartsWith("Before: ["))
        {
            throw new InvalidOperationException("Failed to parse before registers");
        }

        sample.BeforeRegisters = inputLines[0]["Before: [".Length..^1]
            .Split(", ")
            .Select(int.Parse)
            .ToArray();

        // Instruction
        sample.InstructionValues = inputLines[1]
            .Split(' ')
            .Select(int.Parse)
            .ToArray();

        // After registers
        if (!inputLines[2].StartsWith("After:  ["))
        {
            throw new InvalidOperationException("Failed to parse after registers");
        }

        sample.AfterRegisters = inputLines[2]["After:  [".Length..^1]
            .Split(", ")
            .Select(int.Parse)
            .ToArray();

        return sample;
    }
}
