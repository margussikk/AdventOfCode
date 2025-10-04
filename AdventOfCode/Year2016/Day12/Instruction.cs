namespace AdventOfCode.Year2016.Day12;

internal abstract class Instruction
{
    public abstract void Execute(int[] registers, ref int instructionPointer);

    public static Instruction Parse(string line)
    {
        var parts = line.Split(' ');
        return parts[0] switch
        {
            "cpy" => int.TryParse(parts[1], out int value)
                ? new SetRegisterInstruction(value, ConvertToRegisterIndex(parts[2]))
                : new CopyRegisterInstruction(ConvertToRegisterIndex(parts[1]), ConvertToRegisterIndex(parts[2])),
            "inc" => new IncreaseRegisterInstruction(ConvertToRegisterIndex(parts[1])),
            "dec" => new DecreaseRegisterInstruction(ConvertToRegisterIndex(parts[1])),
            "jnz" => int.TryParse(parts[1], out int value)
                ? new JumpValueInstruction(value, int.Parse(parts[2]))
                : new JumpRegisterInstruction(ConvertToRegisterIndex(parts[1]), int.Parse(parts[2])),
            _ => throw new ArgumentException($"Unknown instruction '{parts[0]}'."),
        };
    }

    private static int ConvertToRegisterIndex(string register) => register[0] - 'a';
}
