namespace AdventOfCode.Year2020.Day12;

internal class Instruction
{
    public InstructionType InstructionType { get; private set; }

    public int Argument { get; private set; }

    public static Instruction Parse(string input)
    {
        var instruction = new Instruction
        {
            InstructionType = input[0] switch
            {
                'N' => InstructionType.North,
                'S' => InstructionType.South,
                'E' => InstructionType.East,
                'W' => InstructionType.West,
                'L' => InstructionType.TurnLeft,
                'R' => InstructionType.TurnRight,
                'F' => InstructionType.Forward,
                _ => throw new InvalidOperationException("Invalid instruction character")
            },
            Argument = int.Parse(input[1..])
        };

        return instruction;
    }
}
