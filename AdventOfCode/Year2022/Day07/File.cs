namespace AdventOfCode.Year2022.Day07;

internal class File
{
    public string Name { get; private set; } = "";
    public int Size { get; private set; }

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
