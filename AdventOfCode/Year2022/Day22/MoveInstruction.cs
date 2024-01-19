namespace AdventOfCode.Year2022.Day22;

internal class MoveInstruction(int steps) : IInstruction
{
    public int Steps { get; } = steps;
}
