using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day07;

[Puzzle(2022, 7, "No Space Left On Device")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private Directory _rootDirectory = new();

    public void ParseInput(string[] inputLines)
    {
        Directory? rootDirectory = null;
        Directory? currentDirectory = null;

        for (var lineIndex = 0; lineIndex < inputLines.Length; lineIndex++)
        {
            var line = inputLines[lineIndex];

            if (!Command.IsCommandLine(line))
            {
                throw new InvalidOperationException("Expected a command line");
            }

            var command = Command.Parse(line);
            if (command is ChangeDirectoryCommand changeDirectoryCommand)
            {
                if (changeDirectoryCommand.Parameter == "/")
                {
                    rootDirectory ??= Directory.Root();
                    currentDirectory = rootDirectory;
                }
                else if (currentDirectory != null)
                {
                    if (changeDirectoryCommand.Parameter == "..")
                    {
                        currentDirectory = currentDirectory.Parent;
                    }
                    else
                    {
                        currentDirectory = currentDirectory.Directories
                            .Find(d => d.Name == changeDirectoryCommand.Parameter);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Change directory, but current directory is null");
                }
            }
            else if (command is ListCommand)
            {
                if (currentDirectory == null)
                {
                    throw new InvalidOperationException("List command, but current directory is null");
                }

                lineIndex++; // Skip the current list command line
                for (; lineIndex < inputLines.Length; lineIndex++)
                {
                    line = inputLines[lineIndex];

                    if (Directory.IsDirectoryLine(line))
                    {
                        var directory = Directory.Parse(line);
                        directory.SetParent(currentDirectory);
                        currentDirectory.Directories.Add(directory);
                    }
                    else
                    {
                        var file = File.Parse(line);
                        currentDirectory.Files.Add(file);
                    }

                    if (lineIndex + 1 < inputLines.Length &&
                        Command.IsCommandLine(inputLines[lineIndex + 1]))
                    {
                        break; // End of current list command
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown command");
            }
        }

        _rootDirectory = rootDirectory ?? throw new InvalidOperationException("Couldn't find root directory");
        _rootDirectory.CalculateTotalSize();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _rootDirectory.EnumerateAllDirectories()
            .Where(x => x.TotalSize <= 100_000)
            .Sum(x => x.TotalSize);

        return new PuzzleAnswer(answer, 1390824);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const int totalDiskSpace = 70_000_000;
        const int neededUnusedSpace = 30_000_000;
        var totalUsedSpace = _rootDirectory.TotalSize;
        var neededToFreeSpace = neededUnusedSpace - (totalDiskSpace - totalUsedSpace);

        var answer = _rootDirectory.EnumerateAllDirectories()
            .Where(x => x.TotalSize >= neededToFreeSpace)
            .Min(x => x.TotalSize);

        return new PuzzleAnswer(answer, 7490863);
    }
}