namespace AdventOfCode.Year2020.Day08;

internal class Instruction
{
    public InstructionType InstructionType { get; private set; }
    public int Argument { get; private set; }

    public Instruction GetFixed()
    {
        if (InstructionType == InstructionType.Acc)
        {
            return this;
        }

        return new Instruction
        {
            InstructionType = InstructionType == InstructionType.Nop
                ? InstructionType.Jmp
                : InstructionType.Nop,
            Argument = Argument,
        };
    }

    public static Instruction Parse(string input)
    {
        var instruction = new Instruction();

        var inputParts = input.Split(' ');

        instruction.InstructionType = inputParts[0] switch
        {
            "nop" => InstructionType.Nop,
            "acc" => InstructionType.Acc,
            "jmp" => InstructionType.Jmp,
            _ => throw new InvalidOperationException("Failed to parse instruction type")
        };

        instruction.Argument = int.Parse(inputParts[1].Replace("+", string.Empty));

        return instruction;
    }
}