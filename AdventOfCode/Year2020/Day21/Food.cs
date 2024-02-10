﻿namespace AdventOfCode.Year2020.Day21;

internal class Food
{
    public string[] Ingredients { get; private set; } = [];

    public string[] Allergens { get; private set; } = [];

    public static Food Parse(string input)
    {
        var splits = input.Split(" (contains ");

        var food = new Food
        {
            Ingredients = [.. splits[0].Split(' ')],
            Allergens = splits[1].Replace(")", string.Empty).Split(", ")
        };

        return food;
    }
}
