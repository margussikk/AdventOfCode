using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Utilities.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Day13;

internal partial class ClawMachine
{
    public Vector2D ButtonA { get; set; }

    public Vector2D ButtonB { get; set; }

    public Coordinate2D Prize { get; set; }

    public bool TryWinThePrize(out long buttonAPushes, out long buttonBPushes)
    {
        var matrixElements = new RationalNumber[][]
        {
            [new(ButtonA.DX), new(ButtonB.DX), new(Prize.X)],
            [new(ButtonA.DY), new(ButtonB.DY), new(Prize.Y)]
        };
        var matrix = new Matrix(matrixElements);
        matrix.TransformToReducedRowEchelonForm();

        var linearEquations = matrix.GetLinearEquations();
        var solution = LinearEquationSolver.Solve(linearEquations).FirstOrDefault()
            ?? throw new InvalidOperationException("Solution not found");

        if (!solution[0].IsWholeNumber || !solution[1].IsWholeNumber)
        {
            buttonAPushes = 0;
            buttonBPushes = 0;

            return false;
        }

        buttonAPushes = solution[0].LongValue;
        buttonBPushes = solution[1].LongValue;

        return true;
    }

    public static ClawMachine Parse(string[] inputLines)
    {
        var clawMachine = new ClawMachine();

        if (inputLines.Length != 3)
        {
            throw new InvalidOperationException("Incorrect amount of ClawMachine input lines");
        }

        // Button A
        var matches = ButtonInputLineRegex().Matches(inputLines[0]);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse Button A input line");
        }

        clawMachine.ButtonA = new Vector2D(long.Parse(matches[0].Groups[1].Value), long.Parse(matches[0].Groups[2].Value));

        // Button B
        matches = ButtonInputLineRegex().Matches(inputLines[1]);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse Button B input line");
        }

        clawMachine.ButtonB = new Vector2D(long.Parse(matches[0].Groups[1].Value), long.Parse(matches[0].Groups[2].Value));

        // Prize
        matches = PrizeInputLineRegex().Matches(inputLines[2]);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse Prize input line");
        }

        clawMachine.Prize = new Coordinate2D(long.Parse(matches[0].Groups[1].Value), long.Parse(matches[0].Groups[2].Value));

        return clawMachine;
    }

    [GeneratedRegex(@"Button [A|B]: X\+(\d+), Y\+(\d+)")]
    private static partial Regex ButtonInputLineRegex();

    [GeneratedRegex(@"Prize: X=(\d+), Y=(\d+)")]
    private static partial Regex PrizeInputLineRegex();
}
