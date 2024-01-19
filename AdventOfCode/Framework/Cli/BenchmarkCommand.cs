using AdventOfCode.Framework.Puzzle;
using Spectre.Console.Cli;

namespace AdventOfCode.Framework.Cli;

public class BenchmarkCommand : Command<BenchmarkSettings>
{
    public override int Execute(CommandContext context, BenchmarkSettings settings)
    {
        var puzzleDetails = settings.Day.HasValue
            ? PuzzleDetailsProvider.GetByYearAndDays(settings.Year, [settings.Day.Value])
            : PuzzleDetailsProvider.GetByYear(settings.Year);

        PuzzleSolverManager.BenchmarkPuzzles(puzzleDetails);

        return 0;
    }
}