namespace AdventOfCode.Year2023.Day01;

internal class SpelledDigit
{
    public string Spelling { get; }
    public int Digit { get; }

    public SpelledDigit(string spelling, int digit)
    {
        Spelling = spelling;
        Digit = digit;
    }
}
