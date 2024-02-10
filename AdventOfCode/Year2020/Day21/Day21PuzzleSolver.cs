using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day21;

[Puzzle(2020, 21, "Allergen Assessment")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private List<Food> _foods = [];

    public void ParseInput(string[] inputLines)
    {
        _foods = inputLines.Select(Food.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var allergenIngredients = FindPossibleAllergenIngredients();

        var ingredientsWithAllergens = allergenIngredients.Values
            .SelectMany(_ => _)
            .Distinct()
            .ToList();

        var answer = _foods.Sum(food => food.Ingredients.Except(ingredientsWithAllergens).Count());

        return new PuzzleAnswer(answer, 1829);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var allergenIngredients = FindPossibleAllergenIngredients();

        var ingredientAllergenMatches = new List<IngredientAllergenMatch>();

        while(allergenIngredients.Count > 0)
        {
            var allergenWithOneIngredient = allergenIngredients.FirstOrDefault(kvp => kvp.Value.Count == 1);
            if (allergenWithOneIngredient.Key is null)
            {
                throw new InvalidOperationException("Couldn't find allergen with one ingredient");
            }

            var ingredientAllergenMatch = new IngredientAllergenMatch(allergenWithOneIngredient.Value[0], allergenWithOneIngredient.Key);
            ingredientAllergenMatches.Add(ingredientAllergenMatch);

            allergenIngredients.Remove(ingredientAllergenMatch.Allergen);

            foreach(var value in allergenIngredients.Values)
            {
                value.Remove(ingredientAllergenMatch.Ingredient);
            }            
        }

        var answer = string.Join(',',
            ingredientAllergenMatches.OrderBy(x => x.Allergen)
                                     .Select(x => x.Ingredient));

        return new PuzzleAnswer(answer, "mxkh,gkcqxs,bvh,sp,rgc,krjn,bpbdlmg,tdbcfb");
    }

    private Dictionary<string, List<string>> FindPossibleAllergenIngredients()
    {
        var allergenIngredients = new Dictionary<string, List<string>>();

        foreach (var food in _foods)
        {
            foreach (var allergen in food.Allergens)
            {
                if (allergenIngredients.TryGetValue(allergen, out var ingredients))
                {
                    ingredients = ingredients.Intersect(food.Ingredients).ToList();
                }
                else
                {
                    ingredients = new List<string>(food.Ingredients);
                }

                allergenIngredients[allergen] = ingredients;
            }
        }

        return allergenIngredients;
    }

    private sealed record IngredientAllergenMatch(string Ingredient, string Allergen);
}