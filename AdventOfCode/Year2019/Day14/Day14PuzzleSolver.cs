using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2019.Day14;

[Puzzle(2019, 14, "Space Stoichiometry")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private Dictionary<string, Reaction> _reactions = [];

    public void ParseInput(string[] inputLines)
    {
        _reactions = inputLines.Select(Reaction.Parse)
                               .ToDictionary(x => x.Product.Name);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetConsumedOre(1L);

        return new PuzzleAnswer(answer, 202617);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var maxOres = 1_000_000_000_000L;

        var approximateFuel = maxOres / GetConsumedOre(1L);

        var range = new NumberRange<long>(approximateFuel, approximateFuel * 2); // Assume that 1 * approximation is too low and 2 * approximation is too high. Answer is somewhere in the middle.
        while(range.Length > 2)
        {
            var fuel = (range.Start + range.End) / 2;

            range = GetConsumedOre(fuel) < maxOres
                ? new NumberRange<long>(fuel, range.End)
                : new NumberRange<long>(range.Start, fuel);
        }

        var answer = GetConsumedOre(range.Start) <= maxOres
            ? range.Start
            : range.End;

        return new PuzzleAnswer(answer, 7863863L);
    }

    private long GetConsumedOre(long fuel)
    {
        var chemicalInventories = new Dictionary<string, ChemicalInventory>()
        {
            [ChemicalNames.Ore] = new ChemicalInventory { Produced = long.MaxValue },
            [ChemicalNames.Fuel] = new ChemicalInventory { Consumed = fuel },
        };

        var queue = new Queue<string>();
        queue.Enqueue(ChemicalNames.Fuel);

        while (queue.TryDequeue(out var productName))
        {
            if (!chemicalInventories.TryGetValue(productName, out var productInventory))
            {
                productInventory = new ChemicalInventory();
                chemicalInventories[productName] = productInventory;
            }

            var required = productInventory.Consumed - productInventory.Produced;
            if (required > 0)
            {
                var reaction = _reactions[productName];

                var units = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(required) / reaction.Product.Quantity));
                productInventory.Produced += reaction.Product.Quantity * units;

                foreach (var reactant in reaction.Reactants)
                {
                    if (!chemicalInventories.TryGetValue(reactant.Name, out var reactantInventory))
                    {
                        reactantInventory = new ChemicalInventory();
                        chemicalInventories[reactant.Name] = reactantInventory;
                    }

                    reactantInventory.Consumed += reactant.Quantity * units;

                    queue.Enqueue(reactant.Name);
                }
            }
        }

        return chemicalInventories[ChemicalNames.Ore].Consumed;
    }

    private sealed class ChemicalInventory
    {
        public long Produced { get; set; }
        public long Consumed { get; set; }
    }
}