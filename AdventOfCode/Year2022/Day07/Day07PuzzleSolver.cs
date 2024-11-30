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

        var lineIndex = 0;
        while (lineIndex < inputLines.Length)
        {
            var line = inputLines[lineIndex];

            if (!Command.IsCommandLine(line))
            {
                throw new InvalidOperationException("Expected a command line");
            }

            var command = Command.Parse(line);
            switch (command)
            {
                case ChangeDirectoryCommand { Parameter: "/" }:
                    rootDirectory ??= Directory.Root();
                    currentDirectory = rootDirectory;
                    break;
                case ChangeDirectoryCommand changeDirectoryCommand when currentDirectory != null:
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

                        break;
                    }
                case ChangeDirectoryCommand:
                    throw new InvalidOperationException("Change directory, but current directory is null");
                case ListCommand when currentDirectory == null:
                    throw new InvalidOperationException("List command, but current directory is null");
                case ListCommand:
                    {
                        lineIndex++; // Skip the current list command line
                        while (lineIndex < inputLines.Length)
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

                            lineIndex++;
                        }

                        break;
                    }
                default:
                    throw new InvalidOperationException("Unknown command");
            }

            lineIndex++;
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