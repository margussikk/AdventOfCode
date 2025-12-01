namespace AdventOfCode.Year2017.Day23;

internal class Instruction
{
    public InstructionCode Code { get; private set; }

    public Operand OperandA { get; private set; } = new Operand();

    public Operand OperandB { get; private set; } = new Operand();

    public static Instruction Parse(string input)
    {
        var splits = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var instruction = new Instruction
        {
            Code = splits[0] switch
            {
                "set" => InstructionCode.Set,
                "sub" => InstructionCode.Sub,
                "mul" => InstructionCode.Mul,
                "jnz" => InstructionCode.Jnz,
                _ => throw new InvalidOperationException($"Invalid instruction code: {splits[0]}")
            },
            OperandA = Operand.Parse(splits[1]),
            OperandB = splits.Length == 3
                ? Operand.Parse(splits[2])
                : new Operand(),
        };

        return instruction;
    }
}
