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
            var hasStraight = false;
            for (index = 0; index < password.Length - 3; index++)
            {
                if (password[index + 2] == password[index + 1] + 1 &&
                    password[index + 1] == password[index] + 1)
                {
                    hasStraight = true;
                    break;
                }
            }

            if (!hasStraight)
            {
                continue;
            }

            var hasTwoDifferentPairs = false;
            var startIndexes = new Dictionary<char, int>();
            for (index = 0; index < password.Length - 1; index++)
            {
                if (password[index] != password[index + 1])
                {
                    continue;
                }

                if (startIndexes.TryGetValue(password[index], out var startIndex))
                {
                    if (index > startIndex + 1)
                    {
                        hasTwoDifferentPairs = true;
                        break;
                    }
                }
                else
                {
                    if (startIndexes.Count == 1)
                    {
                        hasTwoDifferentPairs = true;
                        break;
                    }

                    startIndexes.Add(password[index], index);
                }
            }

            if (hasTwoDifferentPairs)
            {
                break;
            }
        }

        return password.JoinToString();
    }
}