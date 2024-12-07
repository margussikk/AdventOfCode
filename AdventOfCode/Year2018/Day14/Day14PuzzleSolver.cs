using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;
using System.Text;

namespace AdventOfCode.Year2018.Day14;

[Puzzle(2018, 14, "Chocolate Charts")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private string _inputRecipe = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _inputRecipe = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        const int recipeCount = 10;

        var recipesOffset = int.Parse(_inputRecipe);

        var scoreboard = new List<int>() { 3, 7 };

        var elf1Position = 0;
        var elf2Position = 1;

        while (scoreboard.Count < recipesOffset + recipeCount)
        {
            var newRecipe = scoreboard[elf1Position] + scoreboard[elf2Position];
            if (newRecipe >= 10)
            {
                scoreboard.Add(1);
            }
            scoreboard.Add(newRecipe % 10);

            elf1Position = MathFunctions.Modulo(elf1Position + scoreboard[elf1Position] + 1, scoreboard.Count);
            elf2Position = MathFunctions.Modulo(elf2Position + scoreboard[elf2Position] + 1, scoreboard.Count);
        }

        var answer = scoreboard.Skip(recipesOffset)
                               .Take(recipeCount)
                               .Aggregate(new StringBuilder(), (agg, curr) => agg.Append(curr))
                               .ToString();

        return new PuzzleAnswer(answer, "3656126723");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var recipeDigits = _inputRecipe.Select(x => x - '0')
                                       .ToList();

        var scoreboard = new List<int>() { 3, 7 };

        var elf1Position = 0;
        var elf2Position = 1;

        while (true)
        {
            var newRecipe = scoreboard[elf1Position] + scoreboard[elf2Position];
            if (newRecipe >= 10)
            {
                scoreboard.Add(1);

                if (Found(scoreboard, recipeDigits))
                {
                    break;
                }
            }

            scoreboard.Add(newRecipe % 10);
            if (Found(scoreboard, recipeDigits))
            {
                break;
            }

            elf1Position = MathFunctions.Modulo(elf1Position + scoreboard[elf1Position] + 1, scoreboard.Count);
            elf2Position = MathFunctions.Modulo(elf2Position + scoreboard[elf2Position] + 1, scoreboard.Count);
        }

        var answer = scoreboard.Count - recipeDigits.Count;

        return new PuzzleAnswer(answer, 20333868);
    }

    private static bool Found(List<int> scoreboard, List<int> recipeDigits)
    {
        if (scoreboard.Count < recipeDigits.Count || scoreboard[^1] != recipeDigits[^1])
        {
            return false;
        }

        for (var i = 0; i < recipeDigits.Count; i++)
        {
            if (scoreboard[^(recipeDigits.Count - i)] != recipeDigits[i])
            {
                return false;
            }
        }

        return true;
    }
}