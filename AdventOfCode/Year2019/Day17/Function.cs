namespace AdventOfCode.Year2019.Day17;

internal class Function(string name, List<string> actions)
{
    public string Name { get; } = name;

    public List<string> Actions { get; } = actions;
}
