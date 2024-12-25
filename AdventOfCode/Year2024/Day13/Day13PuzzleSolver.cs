using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day13;

[Puzzle(2024, 13, "Claw Contraption")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private List<ClawMachine> _clawMachines = [];

    public void ParseInput(string[] inputLines)
    {
        _clawMachines = inputLines.SelectToChunks()
                                  .Select(ClawMachine.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _clawMachines
            .Select(cm => cm.TryWinThePrize(out var numberOfButtonAPushes, out var numberOfButtonBPushes)
                ? (ButtonAPushes: numberOfButtonAPushes, ButtonBPushes: numberOfButtonBPushes)
                : (ButtonAPushes: 0L, ButtonBPushes: 0L))
            .Where(cm => cm.ButtonAPushes <= 100 && cm.ButtonBPushes <= 100)
            .Sum(x => x.ButtonAPushes * 3 + x.ButtonBPushes);

        return new PuzzleAnswer(answer, 27105);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const long prizeIncrease = 10_000_000_000_000L;

        var answer = _clawMachines
            .Select(cm => new ClawMachine
            {
                ButtonA = cm.ButtonA,
                ButtonB = cm.ButtonB,
                Prize = new Coordinate2D(cm.Prize.X + prizeIncrease, cm.Prize.Y + prizeIncrease),
            })
            .Select(cm => cm.TryWinThePrize(out var numberOfButtonAPushes, out var numberOfButtonBPushes)
                ? (ButtonAPushes: numberOfButtonAPushes, ButtonBPushes: numberOfButtonBPushes)
                : (ButtonAPushes: 0L, ButtonBPushes: 0L))
            .Sum(x => x.ButtonAPushes * 3 + x.ButtonBPushes);

        return new PuzzleAnswer(answer, 101726882250942L);
    }
}