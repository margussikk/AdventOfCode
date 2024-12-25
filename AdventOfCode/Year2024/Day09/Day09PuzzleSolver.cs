using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2024.Day09;

[Puzzle(2024, 9, "Disk Fragmenter")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private List<int> _diskMap = [];

    public void ParseInput(string[] inputLines)
    {
        _diskMap = inputLines[0].Select(c => c.ParseToDigit())
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
        var emptyRegionQueues = Enumerable.Range(0, 9).Select(x => new PriorityQueue<DiskRegion, int>()).ToArray();
        var fileRegions = new Stack<DiskRegion>();

        var position = 0;
        for (var index = 0; index < _diskMap.Count; index++)
        {
            var length = _diskMap[index];
            if (length == 0)
            {
                continue;
            }

            var diskRegion = new DiskRegion
            {
                Position = position,
                Length = length
            };

            if (index % 2 == 0) // File
            {
                diskRegion.FileId = index / 2;
                fileRegions.Push(diskRegion);
            }
            else // Empty
            {
                diskRegion.FileId = default;
                emptyRegionQueues[diskRegion.Length - 1].Enqueue(diskRegion, diskRegion.Position);
            }

            position += length;
        }

        var processedFileRegions = new List<DiskRegion>();

        while (fileRegions.TryPop(out var fileRegion))
        {
            var emptyRegionQueue = emptyRegionQueues.Where(x => x.TryPeek(out var emptyRegion, out _) &&
                                                                emptyRegion.Position < fileRegion.Position &&
                                                                emptyRegion.Length >= fileRegion.Length)
                                                    .MinBy(x => x.Peek().Position);
            if (emptyRegionQueue != null && emptyRegionQueue.TryDequeue(out var emptyRegion, out _))
            {
                fileRegion.Position = emptyRegion!.Position;

                if (emptyRegion.Length != fileRegion.Length)
                {
                    emptyRegion.Length -= fileRegion.Length;
                    emptyRegion.Position += fileRegion.Length;

                    emptyRegionQueues[emptyRegion.Length - 1].Enqueue(emptyRegion, emptyRegion.Position);
                }
            }

            processedFileRegions.Add(fileRegion);
        }

        var answer = processedFileRegions.Sum(x => x.Checksum);

        return new PuzzleAnswer(answer, 6353648390778L);
    }

    private sealed class DiskRegion
    {
        public int Position { get; set; }
        public int? FileId { get; set; }
        public int Length { get; set; }

        public long Checksum => FileId.HasValue
            ? new NumberRange<long>(Position, Position + Length - 1).Sum() * FileId.Value
            : default;
    }
}