using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day14;

[Puzzle(2022, 14, "Regolith Reservoir")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    static readonly GridDirection[] _dropDirections =
    [
        GridDirection.Down,
        GridDirection.DownLeft,
        GridDirection.DownRight,
    ];

    private readonly HashSet<GridCoordinate> _rockLocations = [];

    private int _maxRow = int.MinValue;

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var pathParts = line.Split("->", StringSplitOptions.TrimEntries).ToList();
            for (var i = 0; i < pathParts.Count - 1; i++)
            {
                var startCoordinate = GetCoordinate(pathParts[i]);
                var endCoordinate = GetCoordinate(pathParts[i + 1]);

                _maxRow = Math.Max(_maxRow, startCoordinate.Row);
                _maxRow = Math.Max(_maxRow, endCoordinate.Row);

                if (startCoordinate.Row == endCoordinate.Row) // Horizontal line
                {
                    var startColumn = Math.Min(startCoordinate.Column, endCoordinate.Column);
                    var endColumn = Math.Max(startCoordinate.Column, endCoordinate.Column);

                    for (var column = startColumn; column <= endColumn; column++)
                    {
                        _rockLocations.Add(new GridCoordinate(startCoordinate.Row, column));
                    }
                }
                else if (startCoordinate.Column == endCoordinate.Column) // Vertical line
                {
                    var startRow = Math.Min(startCoordinate.Row, endCoordinate.Row);
                    var endRow = Math.Max(startCoordinate.Row, endCoordinate.Row);

                    for (var row = startRow; row <= endRow; row++)
                    {
                        _rockLocations.Add(new GridCoordinate(row, startCoordinate.Column));
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        var tiles = new Dictionary<GridCoordinate, Tile>();
        foreach (var location in _rockLocations)
        {
            tiles.Add(location, Tile.Rock);
        }

        var sandStack = new Stack<GridCoordinate>();
        sandStack.Push(new GridCoordinate(0, 500));

        while (DropSand(1, tiles, sandStack, _maxRow))
        {
            answer++;
        }

        return new PuzzleAnswer(answer, 1072);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        var tiles = new Dictionary<GridCoordinate, Tile>();
        foreach (var location in _rockLocations)
        {
            tiles.Add(location, Tile.Rock);
        }

        var sandStack = new Stack<GridCoordinate>();
        sandStack.Push(new GridCoordinate(0, 500));

        while (DropSand(2, tiles, sandStack, _maxRow + 2))
        {
            answer++;
        }

        return new PuzzleAnswer(answer, 24659);
    }

    private static bool DropSand(int part, Dictionary<GridCoordinate, Tile> tiles, Stack<GridCoordinate> sandStack, int maxRow)
    {
        if (sandStack.Count == 0)
        {
            return false;
        }

        var sandCoordinate = sandStack.Pop();

        while (true)
        {
            var nextSandCoordinate = GetNextSandCoordinate(part, tiles, sandCoordinate, maxRow);
            if (nextSandCoordinate != sandCoordinate)
            {
                // Dropped lower
                if (part == 1 && nextSandCoordinate.Row == maxRow)
                {
                    return false;
                }

                sandStack.Push(sandCoordinate);
                sandCoordinate = nextSandCoordinate;
            }
            else
            {
                // At rest
                tiles[nextSandCoordinate] = Tile.Sand;
                return true;
            }
        }
    }

    private static GridCoordinate GetNextSandCoordinate(int part, Dictionary<GridCoordinate, Tile> tiles, GridCoordinate dropCoordinate, int maxRow)
    {
        if (tiles.GetValueOrDefault(dropCoordinate, Tile.Air) != Tile.Air)
        {
            throw new InvalidOperationException("Couldn't drop, tile was occupied");
        }

        foreach (var dropDirection in _dropDirections)
        {
            var nextCoordinate = dropCoordinate.Move(dropDirection);
            if (part == 2 && nextCoordinate.Row >= maxRow) // Infinite horizontal rock path
            {
                return dropCoordinate;
            }
            else if (tiles.GetValueOrDefault(nextCoordinate, Tile.Air) == Tile.Air)
            {
                return nextCoordinate;
            }
        }

        return dropCoordinate;
    }

    private static GridCoordinate GetCoordinate(string value)
    {
        var splits = value.Split(',');
        return new GridCoordinate(int.Parse(splits[1]), int.Parse(splits[0])); // NB! Input is in Column, Row order
    }
}