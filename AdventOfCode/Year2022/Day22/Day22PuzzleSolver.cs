using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2022.Day22;

[Puzzle(2022, 22, "Monkey Map")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _grid = new(0, 0);

    private readonly List<IInstruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        // Map
        _grid = new Grid<Tile>(chunks[0].Length, chunks[0].Max(l => l.Length));

        for (var row = 0; row < _grid.Height; row++)
        {
            for (var column = 0; column < chunks[0][row].Length; column++)
            {
                _grid[row, column] = chunks[0][row][column] switch
                {
                    ' ' => Tile.Nothing,
                    '.' => Tile.Open,
                    '#' => Tile.Wall,
                    _ => throw new InvalidOperationException()
                };
            }

            for (var column = chunks[0][row].Length; column < _grid.Width; column++)
            {
                _grid[row, column] = Tile.Nothing;
            }
        }

        // Instructions
        var span = chunks[1][0].AsSpan();
        while (span.Length > 0)
        {
            switch (span[0])
            {
                case 'L':
                    _instructions.Add(new TurnInstruction(GridDirection.Left));
                    span = span[1..];
                    break;
                case 'R':
                    _instructions.Add(new TurnInstruction(GridDirection.Right));
                    span = span[1..];
                    break;
                default:
                    {
                        if (char.IsDigit(span[0]))
                        {
                            var lastIndex = span.IndexOfAnyExceptInRange('0', '9');
                            if (lastIndex == -1)
                            {
                                lastIndex = span.Length;
                            }

                            var steps = int.Parse(span[..lastIndex]);
                            _instructions.Add(new MoveInstruction(steps));
                            span = span[lastIndex..];
                        }
                        else
                        {
                            throw new InvalidOperationException("Failed to parse instruction");
                        }

                        break;
                    }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var walker = new FlatWalker(_grid);

        foreach (var instruction in _instructions)
        {
            switch (instruction)
            {
                case TurnInstruction { Direction: GridDirection.Left }:
                    walker.TurnLeft();
                    break;
                case TurnInstruction { Direction: GridDirection.Right }:
                    walker.TurnRight();
                    break;
                case TurnInstruction:
                    throw new InvalidOperationException("Invalid turn direction");
                case MoveInstruction moveInstruction:
                    walker.Move(moveInstruction.Steps);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        var answer = 1000 * (walker.Location.Row + 1) + 4 * (walker.Location.Column + 1) + GetFacingValue(walker.Direction);

        return new PuzzleAnswer(answer, 30552);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var walker = new CubeWalker(_grid);

        foreach (var instruction in _instructions)
        {
            switch (instruction)
            {
                case TurnInstruction { Direction: GridDirection.Left }:
                    walker.TurnLeft();
                    break;
                case TurnInstruction { Direction: GridDirection.Right }:
                    walker.TurnRight();
                    break;
                case TurnInstruction:
                    throw new InvalidOperationException("Invalid turn direction");
                case MoveInstruction moveInstruction:
                    walker.Move(moveInstruction.Steps);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        var answer = 1000 * (walker.GetGridRow() + 1) + 4 * (walker.GetGridColumn() + 1) + GetFacingValue(walker.Direction);

        return new PuzzleAnswer(answer, 184106);
    }

    private static int GetFacingValue(GridDirection direction)
    {
        return direction switch
        {
            GridDirection.Right => 0,
            GridDirection.Down => 1,
            GridDirection.Left => 2,
            GridDirection.Up => 3,
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }
}