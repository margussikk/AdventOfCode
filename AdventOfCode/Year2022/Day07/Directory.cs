namespace AdventOfCode.Year2022.Day07;

internal class Directory
{
    public string Name { get; private set; } = "";

    public Directory? Parent { get; private set; }

    public List<Directory> Directories { get; set; } = [];

    public List<File> Files { get; set; } = [];

    public int TotalSize { get; private set; }

    public void SetParent(Directory parent)
    {
        Parent = parent;
    }

    public IEnumerable<Directory> EnumerateAllDirectories()
    {
        yield return this;

        foreach (var directory in Directories)
        {
            foreach (var dir2 in directory.EnumerateAllDirectories())
            {
                yield return dir2;
            }
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
