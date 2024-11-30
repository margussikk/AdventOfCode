namespace AdventOfCode.Year2022.Day07;

internal class ChangeDirectoryCommand : Command
{
    public string Parameter { get; }

    public ChangeDirectoryCommand(string parameter)
    {
        Parameter = parameter;
    }
}
