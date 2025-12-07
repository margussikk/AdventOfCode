using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2025.Day07;

[Puzzle(2025, 7, "Laboratories")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private GridCoordinate _startCoordinate;
    private IReadOnlySet<GridCoordinate> _splitterCoordinates = new HashSet<GridCoordinate>();
    private int lastRow;

    public void ParseInput(string[] inputLines)
    {
        var splitterCoordinates = new List<GridCoordinate>();

        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                var character = inputLines[row][column];
                if (character == 'S')
                {
                    _startCoordinate = new GridCoordinate(row, column);
                }
                else if (character == '^')
                {
                    splitterCoordinates.Add(new GridCoordinate(row, column));
                }
            }
        }

        _splitterCoordinates = splitterCoordinates.ToHashSet();
        lastRow = inputLines.Length - 1;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        var visited = new HashSet<GridCoordinate>();

        var queue = new Queue<GridCoordinate>();
        queue.Enqueue(_startCoordinate);

        while (queue.TryDequeue(out var currentCoordinate))
        {
            var nextCoordinate = currentCoordinate.Move(GridDirection.Down);
            if (nextCoordinate.Row > lastRow)
            {
                continue;
            }

            if (_splitterCoordinates.Contains(nextCoordinate))
            {
                var splitCoordinates = new GridCoordinate[]
                {
                    nextCoordinate.Move(GridDirection.Left),
                    nextCoordinate.Move(GridDirection.Right)
                };

                var split = false;
                foreach (var splitCoordinate in splitCoordinates.Where(visited.Add))
                {
                    queue.Enqueue(splitCoordinate);
                    split = true;
                }

                if (split)
                {
                    answer++;
                }
            }
            else if (visited.Add(nextCoordinate))
            {
                queue.Enqueue(nextCoordinate);
            }
        }

        return new PuzzleAnswer(answer, 1598);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cache = new Dictionary<GridCoordinate, long>();

        var answer = CountTimelines(_startCoordinate);

        return new PuzzleAnswer(answer, 4509723641302L);

        long CountTimelines(GridCoordinate entryCoordinate)
        {
            if (cache.TryGetValue(entryCoordinate, out var cachedCount))
            {
                return cachedCount;
            }

            var coordinate = entryCoordinate;
            while (!_splitterCoordinates.Contains(coordinate))
            {
                coordinate = coordinate.Move(GridDirection.Down);
                if (coordinate.Row > lastRow)
                {
                    cache[entryCoordinate] = 1;
                    return 1;
                }
            }

            var leftCount = CountTimelines(coordinate.Move(GridDirection.Left));
            var rightCount = CountTimelines(coordinate.Move(GridDirection.Right));
            var count = leftCount + rightCount;

            cache[entryCoordinate] = count;
            return count;
        }
    }
}