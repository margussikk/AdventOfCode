namespace AdventOfCode.Framework.Puzzle;

public class PuzzleDetails(Type solverType, int year, int day, string name)
{
    public Type SolverType { get; } = solverType;
    public int Year { get; } = year;
    public int Day { get; } = day;
    public string Name { get; } = name;
}
