using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day01;

[Puzzle(2023, 1, "Trebuchet?!")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private readonly SpelledDigit[] _spelledDigits =
    [
        new SpelledDigit("one", 1),
        new SpelledDigit("two", 2),
        new SpelledDigit("three", 3),
        new SpelledDigit("four", 4),
        new SpelledDigit("five", 5),
        new SpelledDigit("six", 6),
        new SpelledDigit("seven", 7),
        new SpelledDigit("eight", 8),
        new SpelledDigit("nine", 9)
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

        if (includeSpelledDigits)
        {
            var startSpan = lineSpan[..firstDigitIndex];
            while (startSpan.Length > 0)
            {
                SpelledDigit? foundSpelledDigit = null;

                foreach (var spelledDigit in _spelledDigits)
                {
                    if (startSpan.StartsWith(spelledDigit.Spelling))
                    {
                        foundSpelledDigit = spelledDigit;
                        break;
                    }
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
                    if (endSpan.EndsWith(spelledDigit.Spelling))
                    {
                        foundSpelledDigit = spelledDigit;
                        break;
                    }
                }

                if (foundSpelledDigit != null)
                {
                    lastDigit = foundSpelledDigit.Digit;
                    break;
                }

                endSpan = endSpan[..^1];
            }
        }

        return 10 * firstDigit + lastDigit;
    }
}
