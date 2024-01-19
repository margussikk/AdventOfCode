using AdventOfCode.Framework.Puzzle;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode.Framework.Cli;

public class UiCommand : Command<UiSettings>
{
    private const string _actionBenchmark = "Benchmark puzzles";
    private const string _actionSolve = "Solve puzzles";

    public override int Execute(CommandContext context, UiSettings settings)
    {
        var action = SelectAction();
        var selectedYear = SelectYear();
        var selectedSolvers = SelectSolvers(selectedYear);

        if (action == _actionBenchmark)
        {
            PuzzleSolverManager.BenchmarkPuzzles(selectedSolvers);
        }
        else if (action == _actionSolve)
        {
            PuzzleSolverManager.SolvePuzzles(selectedSolvers);
        }

        return 0;
    }

    private static string SelectAction()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Which [green]action[/] do you want to execute?")
                .PageSize(10)
                .AddChoices(_actionBenchmark, _actionSolve));
    }

    private static int SelectYear()
    {
        var years = PuzzleDetailsProvider.GetYears();

        return AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("Which [green]year[/] do you want to execute?")
                .PageSize(10)
                .AddChoices(years));
    }

    private static List<PuzzleDetails> SelectSolvers(int year)
    {
        var allPuzzlesChoice = new PuzzleChoice(true, 0, year.ToString());

        var solverDetails = PuzzleDetailsProvider.GetByYear(year);

        var puzzleChoices = solverDetails
            .Select(p => new PuzzleChoice(false, p.Day, p.Name))
            .ToArray();

        var selectedPuzzleChoices = AnsiConsole.Prompt(
            new MultiSelectionPrompt<PuzzleChoice>()
                .Title("Which [green]day[/] do you want to execute?")
                .PageSize(28)
                .MoreChoicesText("[grey](Move up and down to reveal more days)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a day, " +
                    "[green]<enter>[/] to accept)[/]")
                .AddChoiceGroup(allPuzzlesChoice, puzzleChoices)
                .UseConverter(ConvertToDisplayString));

        return solverDetails
            .Where(x => selectedPuzzleChoices.Exists(y => y.Day == x.Day))
            .ToList();

        static string ConvertToDisplayString(PuzzleChoice puzzleChoice)
        {
            return puzzleChoice.All
                ? puzzleChoice.Name
                : $"Day {puzzleChoice.Day}: {puzzleChoice.Name}";
        }
    }

    private sealed record PuzzleChoice(bool All, int Day, string Name);
}
