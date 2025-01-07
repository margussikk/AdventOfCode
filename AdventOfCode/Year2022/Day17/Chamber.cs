using AdventOfCode.Utilities.GridSystem;
using System.Text;

namespace AdventOfCode.Year2022.Day17;

internal class Chamber
{
    private const int EmptyLine = 0;

    public List<int> LineBitMasks { get; } = [];

    public void AppearRock(Rock rock)
    {
        const int requiredEmptyLineCount = 3;

        // Count empty lines
        var currentEmptyLineCount = 0;
        var lastNotEmptyLine = LineBitMasks.FindLastIndex(x => x != EmptyLine);
        if (lastNotEmptyLine >= 0)
        {
            currentEmptyLineCount = LineBitMasks.Count - 1 - lastNotEmptyLine;
        }

        switch (currentEmptyLineCount)
        {
            case > requiredEmptyLineCount:
                {
                    // Remove empty lines
                    var excessEmptyLineCount = currentEmptyLineCount - requiredEmptyLineCount;
                    LineBitMasks.RemoveRange(LineBitMasks.Count - excessEmptyLineCount, excessEmptyLineCount);
                    break;
                }
            case < requiredEmptyLineCount:
                {
                    // Add empty lines
                    var missingEmptyLineCount = requiredEmptyLineCount - currentEmptyLineCount;
                    for (var i = 0; i < missingEmptyLineCount; i++)
                    {
                        LineBitMasks.Add(EmptyLine);
                    }

                    break;
                }
        }

        // Empty lines for rock
        for (var i = 0; i < rock.BitMasks.Length; i++)
        {
            LineBitMasks.Add(EmptyLine);
        }

        rock.Row = LineBitMasks.Count - rock.BitMasks.Length;
        rock.Column = 2;
    }

    public bool CanMoveRock(Rock rock, int rowChange, int columnChange)
    {
        for (var row = 0; row < rock.BitMasks.Length; row++)
        {
            // Adjust row
            var newRockRow = rock.Row + row + rowChange;
            if (newRockRow < 0 || newRockRow >= LineBitMasks.Count)
            {
                return false;
            }

            // Adjust column
            var newRockColumn = rock.Column + columnChange;
            if (newRockColumn < 0 || newRockColumn + rock.Width > 7)
            {
                return false;
            }

            // Adjust bits
            var newRockBitMask = columnChange switch
            {
                -1 => rock.BitMasks[row] << 1,
                0 => rock.BitMasks[row],
                1 => rock.BitMasks[row] >> 1,
                _ => throw new NotImplementedException()
            };

            // Check if bits overlap
            if ((LineBitMasks[newRockRow] & newRockBitMask) != 0)
            {
                return false;
            }
        }

        return true;
    }

    public void RestRock(Rock rock)
    {
        for (var row = 0; row < rock.BitMasks.Length; row++)
        {
            LineBitMasks[rock.Row + row] |= rock.BitMasks[row];
        }
    }

    public int GetTowerHeight()
    {
        var emptyLines = 0;

        for (var row = LineBitMasks.Count - 1; row >= 0; row--)
        {
            var bitmask = LineBitMasks[row];
            if (bitmask != EmptyLine)
            {
                break;
            }

            emptyLines++;
        }

        return LineBitMasks.Count - emptyLines;
    }

    public int CalculateHashCode()
    {
        var edges = new HashSet<GridCoordinate>();
        var visited = new HashSet<GridCoordinate>();
        var queue = new Queue<GridCoordinate>();

        var startLocation = new GridCoordinate(LineBitMasks.Count - 1, 6);

        queue.Enqueue(startLocation);

        while (queue.TryDequeue(out var currentLocation))
        {
            if (!visited.Add(currentLocation))
            {
                continue;
            }

            foreach (var neighbor in GetNeighborLocations(currentLocation))
            {
                var bitmask = 0b1_000_000 >> neighbor.Column;
                if ((LineBitMasks[neighbor.Row] & bitmask) == bitmask)
                {
                    var relativeLocation = new GridCoordinate(startLocation.Row - neighbor.Row, startLocation.Column - neighbor.Column);
                    edges.Add(relativeLocation);
                }
                else
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        var hash = new HashCode();

        foreach (var edge in edges.OrderBy(x => x.Row).ThenBy(x => x.Column))
        {
            hash.Add(edge);
        }

        return hash.ToHashCode();

        IEnumerable<GridCoordinate> GetNeighborLocations(GridCoordinate currentLocation)
        {
            foreach (var newLocation in currentLocation.SideNeighbors())
            {
                if (newLocation.Row < 0 || newLocation.Row >= LineBitMasks.Count ||
                    newLocation.Column < 0 || newLocation.Column >= 7)
                {
                    continue;
                }

                yield return newLocation;
            }
        }
    }

    public void Print(Rock rock)
    {
        for (var row = LineBitMasks.Count - 1; row >= 0; row--)
        {
            var stringBuilder = new StringBuilder("|");

            var bitmask = 0b1_000_000;
            for (var column = 0; column < 7; column++)
            {
                if (rock != null &&
                    row >= rock.Row && row < rock.Row + rock.BitMasks.Length &&
                    (rock.BitMasks[row - rock.Row] & bitmask) == bitmask)
                {
                    stringBuilder.Append('@');
                }
                else if ((LineBitMasks[row] & bitmask) == bitmask)
                {
                    stringBuilder.Append('#');
                }
                else
                {
                    stringBuilder.Append('.');
                }

                bitmask >>= 1;
            }

            stringBuilder.Append('|');
            Console.WriteLine(stringBuilder);
        }
        Console.WriteLine("+-------+");
    }
}
