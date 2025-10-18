using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day15;

[Puzzle(2015, 15, "Science for Hungry People")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Ingredient> _ingredients = [];

    public void ParseInput(string[] inputLines)
    {
        _ingredients = [.. inputLines.Select(Ingredient.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CalculateBestScore([], false);

        return new PuzzleAnswer(answer, 18965440);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = CalculateBestScore([], true);

        return new PuzzleAnswer(answer, 15862900);
    }

    private int CalculateBestScore(int[] ingredientAmounts, bool countCalories)
    {
        var bestScore = 0;
        var ingredientsLeft = 100 - ingredientAmounts.Sum();

        if (ingredientAmounts.Length == _ingredients.Count - 1)
        {
            int[] amounts = [.. ingredientAmounts, ingredientsLeft];

            if (countCalories)
            {
                var calories = _ingredients.Select((ingredient, index) => amounts[index] * ingredient.Calories).Sum();
                if (calories != 500)
                {
                    return 0;
                }
            }

            var score = CalculatePropertiesScore(amounts);
            bestScore = Math.Max(bestScore, score);
        }
        else
        {
            for (var amount = 0; amount <= ingredientsLeft; amount++)
            {
                var score = CalculateBestScore([.. ingredientAmounts, amount], countCalories);
                bestScore = Math.Max(bestScore, score);
            }
        }

        return bestScore;
    }

    private int CalculatePropertiesScore(int[] amounts)
    {
        var capacity = _ingredients.Select((ingredient, index) => amounts[index] * ingredient.Capacity).Sum();
        capacity = Math.Max(0, capacity);

        var durability = _ingredients.Select((ingredient, index) => amounts[index] * ingredient.Durability).Sum();
        durability = Math.Max(0, durability);

        var flavor = _ingredients.Select((ingredient, index) => amounts[index] * ingredient.Flavor).Sum();
        flavor = Math.Max(0, flavor);

        var texture = _ingredients.Select((ingredient, index) => amounts[index] * ingredient.Texture).Sum();
        texture = Math.Max(0, texture);

        return capacity * durability * flavor * texture;
    }
}