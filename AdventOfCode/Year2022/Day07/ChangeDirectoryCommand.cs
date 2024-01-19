namespace AdventOfCode.Year2022.Day07;

internal class ChangeDirectoryCommand(string parameter) : Command
{
    public string Parameter { get; } = parameter;
}
