using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using System.Text;

namespace AdventOfCode.Year2016.Day02;

[Puzzle(2016, 2, "Bathroom Security")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<GridDirection[]> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines.Select(inputLines1 => inputLines1.Select(c => c switch
        {
            'U' => GridDirection.Up,
            'D' => GridDirection.Down,
            'L' => GridDirection.Left,
            'R' => GridDirection.Right,
            _ => throw new InvalidOperationException($"Invalid character '{c}' in input.")
        }).ToArray())];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var keypadLines = new string[]
        {
            "123",
            "456",
            "789"
        };

        var answer = GetAnswer(keypadLines);

        return new PuzzleAnswer(answer, "76792");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var keypadLines = new string[]
        {
            "  1  ",
            " 234 ",
            "56789",
            " ABC ",
            "  D  ",
        };

        var answer = GetAnswer(keypadLines);

        return new PuzzleAnswer(answer, "A7AC3");
    }

    private string GetAnswer(string[] keypadLines)
    {
        var answer = new StringBuilder();

        var keypad = keypadLines.SelectToGrid(c => c);
        var startCoordinate = keypad.First(x => x.Object == '5').Coordinate;

        foreach (var instructionSet in _instructions)
        {
            var coordinate = startCoordinate;
            foreach (var instruction in instructionSet)
            {
                var newCoordinate = coordinate.Move(instruction);
                if (keypad.InBounds(newCoordinate) && keypad[newCoordinate] != ' ')
                {
                    coordinate = newCoordinate;
                }
            }
            answer.Append(keypad[coordinate]);
        }

        return answer.ToString();
    }
}