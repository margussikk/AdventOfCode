using AdventOfCode.Framework.Cli;
using Spectre.Console;
using Spectre.Console.Cli;
using System;

namespace AdventOfCode;

public static class Program
{
    static int Main(string[] args)
    {
        var figletText = new FigletText(FigletFont.Default, "Advent of Code")
        {
            Color = ConsoleColor.Green
        };

        AnsiConsole.Write(figletText);

        var app = new CommandApp();

        app.Configure(config =>
        {
            config.AddCommand<BenchmarkCommand>("benchmark")
                  .WithDescription("Benchmarks puzzle(s) in a specified year");

            config.AddCommand<SolveCommand>("solve")
                  .WithDescription("Solves puzzle(s) in a specified year");

            config.AddCommand<UiCommand>("ui")
                  .WithDescription("Opens user interface");
        });

        return app.Run(args);
    }
}
