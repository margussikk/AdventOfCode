namespace AdventOfCode.Year2019.Day14;

internal class Reaction
{
    public Chemical Product { get; private set; } = new(0, string.Empty);
    public List<Chemical> Reactants { get; } = [];

    public static Reaction Parse(string input)
    {
        var reaction = new Reaction();

        var reactionSplits = input.Split(" => ");

        // Reactants
        var reactantsSplits = reactionSplits[0].Split(", ");
        foreach (var reactantString in reactantsSplits)
        {
            var splits = reactantString.Split(' ', StringSplitOptions.TrimEntries);

            var reactantChemical = new Chemical(int.Parse(splits[0]), splits[1]);
            reaction.Reactants.Add(reactantChemical);
        }

        // Product
        var productSplits = reactionSplits[1].Split(' ', StringSplitOptions.TrimEntries);
        reaction.Product = new Chemical(int.Parse(productSplits[0]), productSplits[1]);

        return reaction;
    }
}
