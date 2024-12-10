using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Collections;

namespace AdventOfCode.Year2024.Day09;

[Puzzle(2024, 9, "Disk Fragmenter")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private List<int> _diskMap = [];

    public void ParseInput(string[] inputLines)
    {
        _diskMap = inputLines[0].Select(c => c - '0')
                                .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var diskBlocks = new List<int?>();

        for (var index = 0; index < _diskMap.Count; index++)
        {
            var fileId = index % 2 == 0
                ? index / 2
                : default(int?);

            for (var i = 0; i < _diskMap[index]; i++)
            {
                diskBlocks.Add(fileId);
            }
        }

        var movePosition = diskBlocks.Count - 1;

        for (var position = 0; position < movePosition; position++)
        {
            if (diskBlocks[position] == null)
            {
                while (diskBlocks[movePosition] == null)
                {
                    movePosition--;
                }

                if (movePosition > position)
                {
                    diskBlocks[position] = diskBlocks[movePosition];
                    diskBlocks[movePosition] = null;
                }
            }
        }

        var answer = diskBlocks.Select((id, position) => id.HasValue ? Convert.ToInt64(id!.Value) * position : 0)
                               .Sum();

        return new PuzzleAnswer(answer, 6332189866718);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var emptyRegions = new ListOfLists<DiskRegion>(100);
        var fileRegions = new Stack<DiskRegion>();

        var position = 0;
        for (var index = 0; index < _diskMap.Count; index++)
        {
            var length = _diskMap[index];
            if (length == 0)
            {
                continue;
            }

            if (index % 2 == 0) // File
            {
                fileRegions.Push(new DiskRegion
                {
                    Position = position,
                    FileId = index / 2,
                    Length = length
                });
            }
            else
            {
                var emptyDiskRegion = new DiskRegion
                {
                    Position = position,
                    FileId = default,
                    Length = length
                };

                emptyRegions.Add(emptyDiskRegion);
            }

            position += length;
        }

        var processedFileRegions = new List<DiskRegion>();

        while (fileRegions.TryPop(out var file))
        {
            var emptySpaceIndex = emptyRegions.Find(x => x.Position < file.Position && x.Length >= file.Length, out var emptySpace);
            if (emptySpaceIndex.IsValid)
            {
                file.Position = emptySpace!.Position;

                if (emptySpace.Length == file.Length)
                {
                    emptyRegions.RemoveAt(emptySpaceIndex);
                }
                else
                {
                    emptySpace.Length -= file.Length;
                    emptySpace.Position += file.Length;
                }
            }

            processedFileRegions.Add(file);
        }

        var answer = processedFileRegions.Sum(x => x.Checksum);

        return new PuzzleAnswer(answer, 6353648390778L);
    }

    private sealed class DiskRegion
    {
        public int Position { get; set; }
        public int? FileId { get; set; }
        public int Length { get; set; }

        public long Checksum => FileId.HasValue ? Enumerable.Range(Position, Length).Sum() * Convert.ToInt64(FileId.Value) : default;
    }
}