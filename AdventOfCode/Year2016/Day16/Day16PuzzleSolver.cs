using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Numerics;
using System.Text;

namespace AdventOfCode.Year2016.Day16;

[Puzzle(2016, 16, "Dragon Checksum")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var diskLength = 272;

        var input = GenerateDiskData(_input, diskLength);

        var answer = GenerateChecksum(input, diskLength);

        return new PuzzleAnswer(answer, "10010100110011100");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var diskLength = 35651584;

        var input = GenerateDiskData(_input, diskLength);

        var answer = GenerateChecksum(input, diskLength);

        return new PuzzleAnswer(answer, "01100100101101100");
    }

    private static string GenerateDiskData(string input, int diskLength)
    {
        while (input.Length < diskLength)
        {
            var b = new char[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                b[i] = input[input.Length - 1 - i] == '0' ? '1' : '0';
            }

            input = $"{input}0{new string(b)}";
        }
        return input[..diskLength];
    }

    private static string GenerateChecksum(ReadOnlySpan<char> input, int diskLength)
    {
        var subInputLength = 1;
        var checksumLength = diskLength;

        do
        {
            checksumLength /= 2;
            subInputLength *= 2;

        } while (checksumLength % 2 == 0);

        var stringBuilder = new StringBuilder(checksumLength);

        for (var index = 0; index < diskLength; index += subInputLength)
        {
            var differencies = 0;

            for (var index2 = index; index2 < index + subInputLength; index2 += 2)
            {
                if (input[index2] != input[index2 + 1])
                {
                    differencies++;
                }
            }

            var result = differencies % 2 == 0;

            stringBuilder.Append(result ? '1' : '0');
        }

        return stringBuilder.ToString();
    }

    // Proof of concept for getting the character without generating disk data. Slow.
    private static char GetCharAtIndex(string input, int diskLength, int index)
    {
        var length = input.Length;
        while (length < diskLength)
        {
            length = 2 * length + 1;
        }

        var left = true;

        var numberRange = new NumberRange<int>(0, length - 1);
        while (numberRange.Length > input.Length)
        {
            var middle = (numberRange.Start + numberRange.End) / 2;

            if (index == middle)
            {
                return left ? '0' : '1';
            }
            else if (index < middle)
            {
                numberRange = numberRange.SplitBefore(middle)[0];
                left = true;
            }
            else
            {
                numberRange = numberRange.SplitAfter(middle)[1];
                left = false;
            }
        }

        if (left)
        {
            return input[index - numberRange.Start];
        }
        else
        {
            return input[^(index - numberRange.Start + 1)] == '1' ? '0' : '1';
        }
    }
}