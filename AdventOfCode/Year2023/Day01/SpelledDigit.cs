namespace AdventOfCode.Year2023.Day01;

internal sealed class SpelledDigit(string spelling, int digit)
{
    public string Spelling { get; } = spelling;
    public int Digit { get; } = digit;
}
