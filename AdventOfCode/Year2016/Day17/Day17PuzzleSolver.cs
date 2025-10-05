using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2016.Day17;

[Puzzle(2016, 17, "Two Steps Forward")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private static readonly GridCoordinate _startCoordinate = new(0, 0);
    private static readonly GridCoordinate _endCoordinate = new(3, 3);
    private static readonly GridDirection[] _directions = [GridDirection.Up, GridDirection.Down, GridDirection.Left, GridDirection.Right];
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = string.Empty;

        FollowThePath(Func);

        return new PuzzleAnswer(answer, "RDDRULDDRR");

        bool Func(Walker walker)
        {
            answer = walker.Path;
            return false;
        }
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = int.MinValue;

        FollowThePath(Func);

        return new PuzzleAnswer(answer, 766);

        bool Func(Walker walker)
        {
            answer = int.Max(answer, walker.Path.Length);
            return true;
        }
    }

    private void FollowThePath(Func<Walker, bool> func)
    {
        var walkerQueue = new Queue<Walker>();

        var walker = new Walker
        {
            Coordinate = _startCoordinate,
        };

        walkerQueue.Enqueue(walker);

        while (walkerQueue.TryDequeue(out walker))
        {
            if (walker.Coordinate == _endCoordinate)
            {
                if (!func(walker))
                {
                    return;
                }

                continue;
            }

            var directions = GetOpenDirections($"{_input}{walker.Path}");
            foreach (var direction in directions)
            {
                var nextCoordinate = walker.Coordinate.Move(direction);
                if (nextCoordinate.Row < _startCoordinate.Row || nextCoordinate.Row > _endCoordinate.Row ||
                    nextCoordinate.Column < _startCoordinate.Column || nextCoordinate.Column > _endCoordinate.Column)
                {
                    continue;
                }

                var nextWalker = new Walker
                {
                    Coordinate = nextCoordinate,
                    Path = $"{walker.Path}{GetDirectionCharacter(direction)}",
                };

                walkerQueue.Enqueue(nextWalker);
            }
        }
    }

    private static IEnumerable<GridDirection> GetOpenDirections(string input)
    {
        var hash = ComputeHash(input);

        for (var index = 0; index < 4; index++)
        {
            if (hash[index] is >= 'b' and <= 'f')
            {
                yield return _directions[index];
            }
        }
    }

    private static string ComputeHash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }

    private static char GetDirectionCharacter(GridDirection direction)
    {
        return direction switch
        {
            GridDirection.Up => 'U',
            GridDirection.Down => 'D',
            GridDirection.Left => 'L',
            GridDirection.Right => 'R',
            _ => throw new InvalidOperationException($"Invalid direction {direction}")
        };
    }
}