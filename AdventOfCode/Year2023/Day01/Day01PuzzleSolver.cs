using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day01;

[Puzzle(2023, 1, "Trebuchet?!")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private readonly SpelledDigit[] _spelledDigits =
    [
        new("one", 1),
        new("two", 2),
        new("three", 3),
        new("four", 4),
        new("five", 5),
        new("six", 6),
        new("seven", 7),
        new("eight", 8),
        new("nine", 9)
    ];

    private string[] _inputLines = [];

    public void ParseInput(string[] inputLines)
    {
        _inputLines = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _inputLines
            .Select(x => CalculateCalibrationValue(x, false))
            .Sum();

        return new PuzzleAnswer(answer, 54605);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _inputLines
            .Select(x => CalculateCalibrationValue(x, true))
            .Sum();

        return new PuzzleAnswer(answer, 55429);
    }

    private long CalculateCalibrationValue(string line, bool includeSpelledDigits)
    {
        var lineSpan = line.AsSpan();

        var firstDigitIndex = lineSpan.IndexOfAnyInRange('1', '9');
        var firstDigit = line[firstDigitIndex] - '0';

        var lastDigitIndex = lineSpan.LastIndexOfAnyInRange('1', '9');
        var lastDigit = line[lastDigitIndex] - '0';

        if (!includeSpelledDigits) return 10 * firstDigit + lastDigit;
        
        var startSpan = lineSpan[..firstDigitIndex];
        while (startSpan.Length > 0)
        {
            SpelledDigit? foundSpelledDigit = null;

            foreach (var spelledDigit in _spelledDigits)
            {
                if (!startSpan.StartsWith(spelledDigit.Spelling)) continue;
                    
                foundSpelledDigit = spelledDigit;
                break;
            }

            if (foundSpelledDigit != null)
            {
                firstDigit = foundSpelledDigit.Digit;
                break;
            }

            startSpan = startSpan[1..];
        }

        var endSpan = lineSpan[(lastDigitIndex + 1)..];
        while (endSpan.Length > 0)
        {
            SpelledDigit? foundSpelledDigit = null;

            foreach (var spelledDigit in _spelledDigits)
            {
                if (!endSpan.EndsWith(spelledDigit.Spelling)) continue;
                    
                foundSpelledDigit = spelledDigit;
                break;
            }

            if (foundSpelledDigit != null)
            {
                lastDigit = foundSpelledDigit.Digit;
                break;
            }

            endSpan = endSpan[..^1];
        }

        return 10 * firstDigit + lastDigit;
    }
}
