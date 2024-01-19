using AdventOfCode.Framework.Puzzle;
using Spectre.Console.Cli;

namespace AdventOfCode.Framework.Cli;

public class SolveCommand : Command<SolveSettings>
{
    public override int Execute(CommandContext context, SolveSettings settings)
    {
        var puzzleDetails = settings.Day.HasValue
            ? PuzzleDetailsProvider.GetByYearAndDays(settings.Year, [settings.Day.Value])
            : PuzzleDetailsProvider.GetByYear(settings.Year);

        PuzzleSolverManager.SolvePuzzles(puzzleDetails);

        return 0;
    }
}