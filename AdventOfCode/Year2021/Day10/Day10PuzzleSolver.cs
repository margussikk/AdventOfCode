using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day10;

[Puzzle(2021, 10, "Syntax Scoring")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<char, char> _chunkOpenClosePairs = new()
    {
        ['('] = ')',
        ['['] = ']',
        ['{'] = '}',
        ['<'] = '>'
    };

    private string[] _inputLines = [];

    public void ParseInput(string[] inputLines)
    {
        _inputLines = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        Dictionary<char, int> chunkCloseValues = new()
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137
        };

        var answer = 0;

        foreach (var line in _inputLines)
        {
            var stack = new Stack<char>();

            foreach (var character in line)
            {
                if (_chunkOpenClosePairs.ContainsKey(character))
                {
                    stack.Push(character);
                }
                else if (stack.Count != 0 && _chunkOpenClosePairs[stack.Pop()] == character)
                {
                    // Do nothing
                }
                else
                {
                    answer += chunkCloseValues[character];
                    break;
                }
            }
        }

        return new PuzzleAnswer(answer, 392367);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        Dictionary<char, int> chunkOpenValues = new()
        {
            ['('] = 1,
            ['['] = 2,
            ['{'] = 3,
            ['<'] = 4
        };

        var scores = new List<long>();

        foreach (var line in _inputLines)
        {
            var corrupt = false;
            var stack = new Stack<char>();

            foreach (var character in line)
            {
                if (_chunkOpenClosePairs.ContainsKey(character))
                {
                    stack.Push(character);
                }
                else if (stack.Count != 0 && _chunkOpenClosePairs[stack.Pop()] == character)
                {
                    // Do nothing
                }
                else
                {
                    corrupt = true;
                    break;
                }
            }

            if (corrupt || stack.Count == 0) continue;

            var answer1 = 0L;

            while (stack.Count != 0)
            {
                answer1 *= 5;
                answer1 += chunkOpenValues[stack.Pop()];
            }

            scores.Add(answer1);
        }

        var answer = scores
            .OrderBy(x => x)
            .Skip(scores.Count / 2)
            .First();

        return new PuzzleAnswer(answer, 2192104158L);
    }
}