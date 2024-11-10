using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Spectre.Console;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;

namespace AdventOfCode.Framework.Puzzle;

public static class PuzzleSolverManager
{
    public static void BenchmarkPuzzles(IReadOnlyList<PuzzleDetails> puzzleDetails)
    {
        var benchmarkerTypes = puzzleDetails
                .Select(x => typeof(PuzzleSolverBenchmarker<>).MakeGenericType(x.SolverType))
                .ToArray();

        BenchmarkSwitcher.FromTypes(benchmarkerTypes)
                         .RunAllJoined(DefaultConfig.Instance
                                                    .WithOrderer(new TypeOrderer()));
    }


    public static void SolvePuzzles(IReadOnlyList<PuzzleDetails> puzzleDetails)
    {
        if (puzzleDetails.Count == 0)
        {
            Console.WriteLine("No puzzles found");
            return;
        }

        var table = new Table()
            .AddColumns(
                new TableColumn("[bold]Puzzle[/]").LeftAligned(),
                new TableColumn("[bold]Parse time[/]").RightAligned(),
                new TableColumn("[bold]Part 1 answer[/]").RightAligned(),
                new TableColumn("[bold]Part 1 time[/]").RightAligned(),
                new TableColumn("[bold]Part 2 answer[/]").RightAligned(),
                new TableColumn("[bold]Part 2 time[/]").RightAligned())
            .HeavyBorder()
            .BorderColor(Color.Grey);

        table.Title = new TableTitle($"Year {puzzleDetails[0].Year}");

        AnsiConsole.Live(table)
                   .Start(ctx =>
                   {
                       foreach (var puzzleDetail in puzzleDetails)
                       {
                           var inputLines = PuzzleInputProvider.Instance.GetInputLines(puzzleDetail.SolverType);
                           if (Activator.CreateInstance(puzzleDetail.SolverType) is IPuzzleSolver solver)
                           {
                               try
                               {
                                   var parseStopwatch = Stopwatch.StartNew();
                                   solver.ParseInput(inputLines);
                                   parseStopwatch.Stop();

                                   var partOneStopwatch = Stopwatch.StartNew();
                                   var partOneAnswer = solver.GetPartOneAnswer();
                                   partOneStopwatch.Stop();

                                   var partTwoStopwatch = Stopwatch.StartNew();
                                   var partTwoAnswer = solver.GetPartTwoAnswer();
                                   partTwoStopwatch.Stop();

                                   table.AddRow(
                                       FormatPuzzleName(puzzleDetail),
                                       FormatElapsedTime(parseStopwatch.Elapsed),
                                       FormatAnswer(partOneAnswer),
                                       FormatElapsedTime(partOneStopwatch.Elapsed),
                                       FormatAnswer(partTwoAnswer),
                                       FormatElapsedTime(partTwoStopwatch.Elapsed));
                               }
                               catch (Exception ex)
                               {
                                   AnsiConsole.WriteException(ex);
                               }

                               ctx.Refresh();
                           }
                           else
                           {
                               AnsiConsole.Write("Failed to create solver");
                           }
                       }
                   });
    }

    private static string FormatPuzzleName(PuzzleDetails puzzleDetails)
    {
        return $"Day {puzzleDetails.Day}: {puzzleDetails.Name}";
    }

    private static string FormatAnswer(PuzzleAnswer answer)
    {
        return answer.Answer == answer.ExpectedAnswer
            ? $"[green]{answer.Answer}[/]"
            : $"[red]{answer.Answer}\nexpected {answer.ExpectedAnswer}[/]";
    }

    private static string FormatElapsedTime(TimeSpan timeSpan)
    {
        return timeSpan.TotalMilliseconds switch
        {
            > 1000 => $"{Convert.ToInt32(timeSpan.TotalMilliseconds / 1000)} s {Convert.ToInt32(timeSpan.TotalMilliseconds % 1000)} ms",
            > 1 => string.Create(CultureInfo.InvariantCulture, $"{timeSpan.TotalMilliseconds:0.##} ms"),
            _ => string.Create(CultureInfo.InvariantCulture, $"{timeSpan.TotalMicroseconds:0.##} us")
        };
    }

    private sealed class TypeOrderer : DefaultOrderer
    {
        public override IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary)
        {
            return benchmarksCases.OrderBy(c => c.Descriptor.Type.FullName)
                                  .ThenBy(c => c.Descriptor.MethodIndex);
        }
    }
}
