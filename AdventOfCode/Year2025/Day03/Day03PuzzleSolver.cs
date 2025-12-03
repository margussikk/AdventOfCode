using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2025.Day03;

[Puzzle(2025, 3, "Lobby")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<string> _banks = [];

    public void ParseInput(string[] inputLines)
    {
        _banks = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _banks.Sum(bank => GetLargestJoltage(bank, 2));

        return new PuzzleAnswer(answer, 17092L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _banks.Sum(bank => GetLargestJoltage(bank, 12));

        return new PuzzleAnswer(answer, 170147128753455L);
    }

    private static long GetLargestJoltage(string bank, int batteriesCount)
    {
        var joltage = new char[batteriesCount];

        for (var batteryIndex = 0; batteryIndex < bank.Length; batteryIndex++)
        {
            var startIndex = joltage.Length - Math.Min(joltage.Length, bank.Length - batteryIndex);

            var turnOnBatteryIndex = Array.FindIndex(joltage, startIndex, c => c < bank[batteryIndex]);
            if (turnOnBatteryIndex >= 0 && turnOnBatteryIndex < joltage.Length)
            {
                joltage[turnOnBatteryIndex] = bank[batteryIndex];
                for (var i = turnOnBatteryIndex + 1; i < joltage.Length; i++)
                {
                    joltage[i] = '0';
                }
            }
        }

        return long.Parse(new string(joltage));
    }
}