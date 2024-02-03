using AdventOfCode.Framework.Puzzle;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode.Year2020.Day07;

[Puzzle(2020, 7, "Handy Haversacks")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private Bag? _shinyGoldBag;

    public void ParseInput(string[] inputLines)
    {
        Dictionary<string, Bag> bags = [];

        foreach (var line in inputLines)
        {
            var lineSplits = line.Split(" bags contain ");

            if (!bags.TryGetValue(lineSplits[0], out var containerBag))
            {
                containerBag = new Bag(lineSplits[0]);
                bags[lineSplits[0]] = containerBag;
            }

            var containmentSplits = lineSplits[1]
                .Replace(".", string.Empty)
                .Replace(" bags", string.Empty)
                .Replace(" bag", string.Empty)
                .Split(", ");

            foreach (var containmentString in containmentSplits.Where(s => s != "no other"))
            {
                var spaceIndex = containmentString.IndexOf(' ');

                var containedBagAmount = int.Parse(containmentString[..spaceIndex]);
                var containedBagColor = containmentString[(spaceIndex + 1)..];

                if (!bags.TryGetValue(containedBagColor, out var containedBag))
                {
                    containedBag = new Bag(containedBagColor);
                    bags[containedBagColor] = containedBag;
                }

                var containment = new Containment(containedBagAmount, containedBag);
                containerBag.AddContainment(containment);

                containedBag.AddContainer(containerBag);
            }
        }

        _shinyGoldBag = bags["shiny gold"];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        if (_shinyGoldBag is null)
        {
            throw new InvalidOperationException("Shing gold bag is null");
        }

        var countedBags = new HashSet<string>();

        var stack = new Stack<Bag>();
        stack.Push(_shinyGoldBag);

        while (stack.TryPop(out var bag))
        {
            if (countedBags.Contains(bag.Color))
            {
                continue;
            }

            countedBags.Add(bag.Color);

            foreach (var newBag in bag.Containers)
            {
                stack.Push(newBag);
            }
        }

        var answer = countedBags.Count - 1; // Don't count "shiny gold" bag

        return new PuzzleAnswer(answer, 103);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        if (_shinyGoldBag is null)
        {
            throw new InvalidOperationException("Shing gold bag is null");
        }

        var answer = CountContainments(_shinyGoldBag);

        return new PuzzleAnswer(answer, 1469);
    }

    private static int CountContainments(Bag bag)
    {
        return bag.Containments
            .Sum(c => c.Amount + c.Amount * CountContainments(c.Bag)); // Count bags and bags inside those bags
    }
}