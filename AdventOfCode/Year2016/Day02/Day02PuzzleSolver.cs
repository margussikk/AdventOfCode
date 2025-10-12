using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using System.Text;

namespace AdventOfCode.Year2016.Day02;

[Puzzle(2016, 2, "Bathroom Security")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<GridDirection[]> _instructionSets = [];

    public void ParseInput(string[] inputLines)
    {
        _instructionSets = [.. inputLines.Select(inputLines1 => inputLines1.Select(c => c.ParseLetterToGridDirection()).ToArray())];
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
        var startCoordinate = keypad.FindCoordinate(x => x == '5') ?? throw new InvalidOperationException("Start coordinate (button 5) not found.");

        foreach (var instructionSet in _instructionSets)
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