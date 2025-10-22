using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2015.Day05;

[Puzzle(2015, 5, "Doesn't He Have Intern-Elves For This?")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private readonly IReadOnlyList<char> _vowelsForPartOne = ['a', 'e', 'i', 'o', 'u'];
    private readonly IReadOnlyList<string> _disallowedStringsForPartTwo = ["ab", "cd", "pq", "xy"];
    private IReadOnlyList<string> _inputStrings = [];

    public void ParseInput(string[] inputLines)
    {
        _inputStrings = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _inputStrings.Count(IsNicePartOne);

        return new PuzzleAnswer(answer, 236);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _inputStrings.Count(IsNicePartTwo);

        return new PuzzleAnswer(answer, 51);
    }

    private bool IsNicePartOne(string inputString)
    {
        // Rule: It contains at least three vowels
        if (inputString.Count(_vowelsForPartOne.Contains) < 3)
        {
            return false;
        }

        // Rule: It contains at least one letter that appears twice in a row
        if (!inputString.SlidingWindow(2).Any(x => x[0] == x[1]))
        {
            return false;
        }

        // Rule: It does not contain the disallowed strings
        return !inputString.SlidingWindow(2)
                           .Select(window => window.JoinToString())
                           .Intersect(_disallowedStringsForPartTwo)
                           .Any();
    }

    private static bool IsNicePartTwo(string inputString)
    {
        // Rule: It contains a pair of any two letters that appears at least twice in the string without overlapping
        var hasPairTwice = inputString.SlidingWindow(2)
                                      .Select((window, index) => new
                                      {
                                          Pair = window.JoinToString(),
                                          Index = index
                                      })
                                      .GroupBy(x => x.Pair)
                                      .Where(group => group.Count() >= 2)
                                      .Any(group => group.Max(x => x.Index) - group.Min(x => x.Index) > 1);

        if (!hasPairTwice)
        {
            return false;
        }

        // Rule: It contains at least one letter which repeats with exactly one letter between them
        return inputString.SlidingWindow(3)
                          .Any(window => window[0] == window[2]);
    }
}