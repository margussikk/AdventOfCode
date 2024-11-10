namespace AdventOfCode.Year2022.Day07;

internal class Directory
{
    public string Name { get; private init; } = string.Empty;

    public Directory? Parent { get; private set; }

    public List<Directory> Directories { get; } = [];

    public List<File> Files { get; } = [];

    public int TotalSize { get; private set; }

    public void SetParent(Directory parent)
    {
        Parent = parent;
    }

    public IEnumerable<Directory> EnumerateAllDirectories()
    {
        yield return this;

        foreach (var dir2 in Directories.SelectMany(directory => directory.EnumerateAllDirectories()))
        {
            yield return dir2;
        }
    }

    public int CalculateTotalSize()
    {
        TotalSize = Directories.Sum(x => x.CalculateTotalSize()) +
                    Files.Sum(x => x.Size);

        return TotalSize;
    }

    public static bool IsDirectoryLine(string line)
    {
        return line.StartsWith("dir ");
    }

    public static Directory Root()
    {
        return new Directory
        {
            Name = "/"
        };
    }

    public static Directory Parse(string line)
    {
        var splits = line.Split(' ');

        return new Directory
        {
            Name = splits[1]
        };
    }
}
