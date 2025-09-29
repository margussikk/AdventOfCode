using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2016.Day09;

[Puzzle(2016, 9, "Explosives in Cyberspace")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetDecompressedLength(_input.AsSpan(), false);

        return new PuzzleAnswer(answer, 98135L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetDecompressedLength(_input.AsSpan(), true);

        return new PuzzleAnswer(answer, 10964557606L);
    }

    private static long GetDecompressedLength(ReadOnlySpan<char> input, bool version2)
    {
        var length = 0L;

        while (input.Length != 0)
        {
            var openBracketIndex = input.IndexOf('(');
            if (openBracketIndex == -1)
            {
                break;
            }

            var closeBracketIndex = input.IndexOf(')');
            if (closeBracketIndex == -1)
            {
                break;
            }

            // Add the length of the characters before the next marker
            length += openBracketIndex;

            var marker = input[(openBracketIndex + 1)..closeBracketIndex];
            var separatorIndex = marker.IndexOf('x');
            var charsToTake = int.Parse(marker[..separatorIndex]);
            var repeatCount = int.Parse(marker[(separatorIndex + 1)..]);

            var sequenceEndIndex = closeBracketIndex + 1 + charsToTake;
            var sequence = input[(closeBracketIndex + 1)..sequenceEndIndex];

            var sublength = version2 ? GetDecompressedLength(sequence, true) : sequence.Length;

            length += sublength * repeatCount;
            input = input[sequenceEndIndex..];
        }
        length += input.Length;

        return length;
    }
}