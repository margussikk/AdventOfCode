namespace AdventOfCode.Year2019.Day17;

internal class Function
{
    public string Name { get; }

    public List<string> Actions { get; }

    public Function(string name, List<string> actions)
    {
        Name = name;
        Actions = actions;
    }
}
