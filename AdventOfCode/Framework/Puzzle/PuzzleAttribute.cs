namespace AdventOfCode.Framework.Puzzle;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PuzzleAttribute(int year, int day, string name) : Attribute
{
    public int Year { get; } = year;
    public int Day { get; } = day;
    public string Name { get; } = name;
}