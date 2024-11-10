namespace AdventOfCode.Year2022.Day22;

internal class MoveInstruction : IInstruction
{
    public int Steps { get; }

    public MoveInstruction(int steps)
    {
        Steps = steps;
    }
}
