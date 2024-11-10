namespace AdventOfCode.Year2022.Day07;

internal abstract class Command
{
    public static bool IsCommandLine(string line)
    {
        return line.StartsWith('$');
    }

    public static Command Parse(string line)
    {
        var splits = line.Split(' ');
        return splits[1] switch
        {
            "cd" => new ChangeDirectoryCommand(splits[2]),
            "ls" => new ListCommand(),
            _ => throw new InvalidOperationException()
        };
    }
}
