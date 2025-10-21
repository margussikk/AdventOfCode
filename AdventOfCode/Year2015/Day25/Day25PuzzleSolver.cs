using AdventOfCode.Framework.Puzzle;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015.Day25;

[Puzzle(2015, 25, "Let It Snow")]
public partial class Day25PuzzleSolver : IPuzzleSolver
{
    private int _row;
    private int _column;

    public void ParseInput(string[] inputLines)
    {
        var matches = InputLineRegex().Matches(inputLines[0]);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        _row = int.Parse(match.Groups[1].Value);
        _column = int.Parse(match.Groups[2].Value);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var codeIndex = 1;

        for (var row = 1; row < _row; row++)
        {
            codeIndex += row;
        }

        for (var column = 1; column < _column; column++)
        {
            codeIndex += column + _row;
        }

        var answer = 20151125L;

        for (var i = 1; i < codeIndex; i++)
        {
            answer = (answer * 252533) % 33554393L;
        }

        return new PuzzleAnswer(answer, 8997277L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }

    [GeneratedRegex(@"To continue, please consult the code grid in the manual.  Enter the code at row (\d+), column (\d+).")]
    private static partial Regex InputLineRegex();
}