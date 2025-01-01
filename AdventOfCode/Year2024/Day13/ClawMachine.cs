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
        var matrix = new Matrix<double>(2, 3);
        matrix.SetRow(0, [ButtonA.DX, ButtonB.DX, Prize.X]);
        matrix.SetRow(1, [ButtonA.DY, ButtonB.DY, Prize.Y]);

        if (!LinearEquationSolver.TrySolveLinearEquation(matrix, out var doubleValues))
        {
            buttonAPushes = 0;
            buttonBPushes = 0;

            return false;
        }

        buttonAPushes = Convert.ToInt64(doubleValues[0]);
        buttonBPushes = Convert.ToInt64(doubleValues[1]);

        var verifyA = ButtonA.DX * buttonAPushes + ButtonB.DX * buttonBPushes == Prize.X;
        var verifyB = ButtonA.DY * buttonAPushes + ButtonB.DY * buttonBPushes == Prize.Y;

        if (!verifyA || !verifyB)
        {
            buttonAPushes = 0;
            buttonBPushes = 0;

            return false;
        }

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
