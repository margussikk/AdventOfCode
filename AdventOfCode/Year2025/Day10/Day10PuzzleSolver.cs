using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2025.Day10;

[Puzzle(2025, 10, "Factory")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Machine> _machines = [];

    public void ParseInput(string[] inputLines)
    {
        _machines = [.. inputLines.Select(Machine.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _machines.Sum(machine => ConfigureLights(machine, 0, 0));

        return new PuzzleAnswer(answer, 578);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("TODO", "TODO");
    }

    private static int ConfigureLights(Machine machine, int currentValue, int buttonWiring)
    {
        if (currentValue == machine.IndicatorLightsBitmask)
        {
            return 0;
        }

        if (buttonWiring >= machine.ButtonWirings.Length)
        {
            return -1;
        }

        var buttonPresses = new List<int>();

        // Don't press the button
        var noPressValue = ConfigureLights(machine, currentValue, buttonWiring + 1);
        if (noPressValue != -1)
        {
            buttonPresses.Add(noPressValue);
        }

        // Press the button
        var pressValue = ConfigureLights(machine, currentValue ^ machine.ButtonWirings[buttonWiring].Bitmask, buttonWiring + 1);
        if (pressValue != -1)
        {
            buttonPresses.Add(pressValue + 1);
        }

        if (buttonPresses.Count == 0)
        {
            return -1;
        }

        return buttonPresses.Min();
    }
}