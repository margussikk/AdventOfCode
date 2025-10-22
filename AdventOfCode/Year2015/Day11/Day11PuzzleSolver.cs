using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2015.Day11;

[Puzzle(2015, 11, "Corporate Policy")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private string _currentPassword = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _currentPassword = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GenerateNextPassword(_currentPassword);

        return new PuzzleAnswer(answer, "vzbxxyzz");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GenerateNextPassword(GenerateNextPassword(_currentPassword));

        return new PuzzleAnswer(answer, "vzcaabcc");
    }

    private static string GenerateNextPassword(string input)
    {
        var notAllowedLetters = new char[] { 'i', 'o', 'l' };

        var password = input.ToArray();

        // In case input password contains illegal letters, then "fix" it.
        var index = Array.FindIndex(password, x => notAllowedLetters.Contains(x));
        if (index >= 0)
        {
            password[index]++;
            for (var i = index + 1; i < password.Length; i++)
            {
                password[i] = 'a';
            }
        }

        while (true)
        {
            // Generate the password
            index = password.Length - 1;
            while (index >= 0)
            {
                password[index]++;
                if (password[index] > 'z')
                {
                    password[index] = 'a';
                    index--;
                }
                else if (!notAllowedLetters.Contains(password[index]))
                {
                    break;
                }
            }

            // Validate
            var hasStraight = password.SlidingWindow(3)
                                      .Any(window => window[2] == window[1] + 1 &&
                                                     window[1] == window[0] + 1);

            if (!hasStraight)
            {
                continue;
            }

            var pairIndexes = password.SlidingWindow(2)
                                      .Select((window, index) => new
                                      {
                                          Pair = window.JoinToString(),
                                          Index = index
                                      })
                                      .Where(x => x.Pair[0] == x.Pair[1])
                                      .GroupBy(x => x.Pair)
                                      .ToDictionary(g => g.Key, g => g.Select(x => x.Index).ToList());

            if (pairIndexes.Count == 2) // Two different pairs
            {
                break;
            }
            else if (pairIndexes.Count == 1) // Single pair, check if it occurs more than twice
            {
                var indexes = pairIndexes.First().Value;
                if (indexes.Max() > indexes.Min() + 1)
                {
                    break;
                }
            }
        }

        return password.JoinToString();
    }
}