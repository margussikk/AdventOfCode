using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day08;

[Puzzle(2015, 8, "Matchsticks")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<string> _inputStrings = [];

    public void ParseInput(string[] inputLines)
    {
        _inputStrings = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _inputStrings.Sum(code => code.Length - GetInMemoryLength(code));

        return new PuzzleAnswer(answer, 1371);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _inputStrings.Sum(inMemory => GetCodeLength(inMemory) - inMemory.Length);

        return new PuzzleAnswer(answer, 2117);
    }

    private static int GetInMemoryLength(ReadOnlySpan<char> code)
    {
        var length = 0;

        while (code.Length > 0)
        {
            if (code[0] == '"')
            {
                code = code[1..];
                continue; // Don't include start and end "" in length
            }
            else if (code[0] == '\\' && code[1] is '"' or '\\')
            {
                code = code[2..];
            }
            else if (code[0] == '\\' && code[1] == 'x')
            {
                code = code[4..];
            }
            else
            {
                code = code[1..];
            }

            length++;
        }

        return length;
    }

    private static int GetCodeLength(ReadOnlySpan<char> inMemory)
    {
        var length = 2; // For start and end "

        while (inMemory.Length > 0)
        {
            length += inMemory[0] is '"' or '\\' ? 2 : 1;
            inMemory = inMemory[1..];
        }

        return length;
    }
}