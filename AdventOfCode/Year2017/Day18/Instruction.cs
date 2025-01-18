namespace AdventOfCode.Year2017.Day18;
internal class Instruction
{
    public InstructionCode Code {  get; private set; }

    public Operand OperandA { get; private set; } = new Operand();

    public Operand OperandB { get; private set; } = new Operand();

    public static Instruction Parse(string input)
    {
        var splits = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var instruction = new Instruction
        {
            Code = splits[0] switch
            {
                "snd" => InstructionCode.Snd,
                "set" => InstructionCode.Set,
                "add" => InstructionCode.Add,
                "mul" => InstructionCode.Mul,
                "mod" => InstructionCode.Mod,
                "rcv" => InstructionCode.Rcv,
                "jgz" => InstructionCode.Jgz,
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
