using AdventOfCode.Framework.Puzzle;
using System.Text;

namespace AdventOfCode.Year2015.Day10;

[Puzzle(2015, 10, "Elves Look, Elves Say")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(40);

        return new PuzzleAnswer(answer, 360154);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(50);

        return new PuzzleAnswer(answer, 5103798);
    }

    private int GetAnswer(int times)
    {
        var input = _input;

        for (var i = 0; i < times; i++)
        {
            input = LookAndSay(input);
        }

        return input.Length;
    }

    private static string LookAndSay(string input)
    {
        var stringBuilder = new StringBuilder();

        var prevCharacter = '\0';
        var characterCount = 0;

        foreach (var character in input)
        {
            if (character == prevCharacter)
            {
                characterCount++;
            }
            else
            {
                if (prevCharacter != '\0')
                {
                    stringBuilder.Append($"{characterCount}{prevCharacter}");
                }

                prevCharacter = character;
                characterCount = 1;
            }
        }

        if (prevCharacter != '\0')
        {
            stringBuilder.Append($"{characterCount}{prevCharacter}");
        }

        return stringBuilder.ToString();
    }
}