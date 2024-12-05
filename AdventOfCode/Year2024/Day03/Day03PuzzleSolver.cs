using AdventOfCode.Framework.Puzzle;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Day03;

[Puzzle(2024, 3, "Mull It Over")]
public partial class Day03PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = string.Join(string.Empty, inputLines);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var matches = PartOneInputLineRegex().Matches(_input);

        var answer = Enumerable.Range(0, matches.Count)
                               .Sum(i => Multiply(matches[i].Value));

        return new PuzzleAnswer(answer, 189600467);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;
        var doMultiply = true;

        var matches = PartTwoInputLineRegex().Matches(_input);

        for (var i = 0; i < matches.Count; i++)
        {
            var command = matches[i].Value;

            if (command == "do()")
            {
                doMultiply = true;
            }
            else if (command == "don't()")
            {
                doMultiply = false;
            }
            else if (doMultiply)
            {
                answer += Multiply(command);
            }
        }

        return new PuzzleAnswer(answer, 107069718);
    }

    private static int Multiply(string command)
    {
        return command["mul(".Length..^1]
            .Split(',')
            .Select(int.Parse)
            .Aggregate(1, (acc, curr) => acc * curr);
    }

    [GeneratedRegex(@"mul\(\d{1,3}\,\d{1,3}\)")]
    private static partial Regex PartOneInputLineRegex();

    [GeneratedRegex(@"(mul\(\d{1,3}\,\d{1,3}\))|(do\(\))|(don't\(\))")]
    private static partial Regex PartTwoInputLineRegex();
}