using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using System.Text;

namespace AdventOfCode.Year2017.Day19;

[Puzzle(2017, 19, "A Series of Tubes")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private Grid<GridDirection> _diagram = new(0, 0);
    private readonly Dictionary<GridCoordinate, char> _letterCoordinates = [];

    public void ParseInput(string[] inputLines)
    {
        _diagram = inputLines.SelectToGrid((character, coordinate) =>
        {
            if (character == ' ')
            {
                return GridDirection.None;
            }
            else if (character == '|')
            {
                return GridDirection.UpAndDown;
            }
            else if (character == '-')
            {
                return GridDirection.LeftAndRight;
            }
            else if (character == '+' || char.IsLetter(character))
            {
                if (char.IsLetter(character))
                {
                    _letterCoordinates[coordinate] = character;
                }

                var direction = GridDirection.AllSides;

                foreach(var neighborCoordinate in coordinate.SideNeighbors().Where(c => inputLines[c.Row][c.Column] == ' '))
                {
                    direction = direction.Clear(coordinate.DirectionToward(neighborCoordinate));
                }

                return direction;
            }
            else
            {
                throw new InvalidOperationException($"Invalid input character: {character}");
            }
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        GetAnswer(out var answer, out _);

        return new PuzzleAnswer(answer, "MOABEUCWQS");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        GetAnswer(out _, out var answer);

        return new PuzzleAnswer(answer, 18058);
    }

    private void GetAnswer(out string letters, out int steps)
    {
        var startCoordinate = _diagram.Row(_diagram.FirstRow)
                                      .First(x => x.Object == GridDirection.UpAndDown)
                                      .Coordinate;

        var gridWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.Down, 1);

        var lettersBuilder = new StringBuilder();

        while (true)
        {
            if (_letterCoordinates.TryGetValue(gridWalker.Coordinate, out var letter))
            {
                lettersBuilder.Append(letter);

                if (_diagram[gridWalker.Coordinate].Clear(gridWalker.Direction.Flip()) == GridDirection.None)
                {
                    // Reached the end
                    break;
                }
            }

            if (_diagram[gridWalker.Coordinate] is GridDirection.UpAndLeft or GridDirection.UpAndRight or GridDirection.DownAndLeft or GridDirection.DownAndRight)
            {
                var newDirection = _diagram[gridWalker.Coordinate].Clear(gridWalker.Direction.Flip());
                gridWalker.Move(newDirection);
            }
            else
            {
                gridWalker.Step();
            }
        }

        letters = lettersBuilder.ToString();
        steps = gridWalker.Steps;
    }
}