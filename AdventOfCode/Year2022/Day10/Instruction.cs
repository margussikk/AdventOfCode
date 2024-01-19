namespace AdventOfCode.Year2022.Day10;

internal abstract class Instruction
{
    public int CyclesCount { get; protected set; }

    public static Instruction Parse(string line)
    {
        var splits = line.Split(' ');
        if (splits[0] == "noop")
        {
            return new NoOpInstruction();
        }
        else if (splits[0] == "addx")
        {
            return new AddXInstruction(int.Parse(splits[1]));
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
