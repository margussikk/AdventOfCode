namespace AdventOfCode.Year2022.Day07;

internal class File
{
    public string Name { get; private init; } = string.Empty;
    public int Size { get; private init; }

    public static File Parse(string line)
    {
        var splits = line.Split(' ');

        return new File
        {
            Size = int.Parse(splits[0]),
            Name = splits[1]
        };
    }
}
