using Spectre.Console.Cli;
using System.ComponentModel;

namespace AdventOfCode.Framework.Cli;

public class BenchmarkSettings : CommandSettings
{
    [Description("Year of the Advent of Code")]
    [CommandArgument(0, "<YEAR>")]
    public int Year { get; set; }

    [Description("Day of the puzzle. If not specified, then all puzzles are benchmarked for the specified year.")]
    [CommandOption("-d|--day <DAY>")]
    public int? Day { get; set; }
}
