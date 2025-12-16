using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Utilities.Numerics;
using Spectre.Console;

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
        var answer = 0L;

        foreach (var machine in _machines)
        {
            // Build matrix
            var matrixElements = Enumerable
                .Range(0, machine.JoltageRequirements.Length)
                .Select(joltageIndex => Enumerable
                    .Range(0, machine.Buttons.Length)
                    .Select(buttonIndex => machine.Buttons[buttonIndex].Wirings.Contains(joltageIndex) ? 1L : 0)
                    .Append(machine.JoltageRequirements[joltageIndex])
                    .Select(x => new RationalNumber(x))
                    .ToArray())
                .ToArray();

            var matrix = new Matrix(matrixElements);
            matrix.TransformToReducedRowEchelonForm();

            // Solve linear equations
            var equations = matrix.GetLinearEquations();

            answer += LinearEquationSolver
                .Solve(equations, 0, machine.JoltageRequirements.Max())
                .Where(x => x.All(x => x.IsWholeNumber))
                .Min(x => x.Aggregate((curr, agg) => curr + agg).LongValue);
        }

        return new PuzzleAnswer(answer, 20709);
    }

    private static int ConfigureLights(Machine machine, int currentValue, int buttonWiring)
    {
        if (currentValue == machine.IndicatorLightsBitmask)
        {
            return 0;
        }

        if (buttonWiring >= machine.Buttons.Length)
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
        var pressValue = ConfigureLights(machine, currentValue ^ machine.Buttons[buttonWiring].WiringBitmask, buttonWiring + 1);
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