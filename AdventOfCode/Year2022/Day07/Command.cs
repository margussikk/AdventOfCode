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
        if (splits[1] == "cd")
        {
            return new ChangeDirectoryCommand(splits[2]);
        }
        else if (splits[1] == "ls")
        {
            return new ListCommand();
        }

        throw new InvalidOperationException();
    }
}
