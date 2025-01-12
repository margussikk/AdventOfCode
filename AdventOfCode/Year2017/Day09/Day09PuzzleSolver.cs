using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day09;

[Puzzle(2017, 9, "Stream Processing")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private string _stream = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _stream = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        ProcessStream(out var answer, out _);

        return new PuzzleAnswer(answer, 15922);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        ProcessStream(out _, out var answer);

        return new PuzzleAnswer(answer, 7314);
    }

    private void ProcessStream(out int totalGroupsScore, out int garbageCharacters)
    {
        totalGroupsScore = 0;
        garbageCharacters = 0;

        var groupDepth = 0;
        var insideGarbage = false;
        var ignoreCharacter = false;

        foreach (var character in _stream)
        {
            if (insideGarbage)
            {
                if (ignoreCharacter)
                {
                    ignoreCharacter = false;
                }
                else if (character == '!')
                {
                    ignoreCharacter = true;
                }
                else if (character == '>')
                {
                    insideGarbage = false;
                }
                else
                {
                    garbageCharacters++;
                }
            }
            else if (character == '{')
            {
                groupDepth++;
                totalGroupsScore += groupDepth;
            }
            else if (character == '}')
            {
                groupDepth--;
            }
            else if (character == '<')
            {
                insideGarbage = true;
            }
        }
    }
}