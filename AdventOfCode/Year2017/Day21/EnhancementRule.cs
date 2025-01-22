using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2017.Day21;
internal class EnhancementRule
{
    public HashSet<int> Patterns { get; private set; } = [];
    public Grid<bool> Output { get; private set; } = new(0, 0);

    public static EnhancementRule Parse(string input)
    {
        var enchantmentRule = new EnhancementRule();

        var splits = input.Split("=>", StringSplitOptions.TrimEntries);

        var patternGrid = splits[0].Split('/').SelectToGrid(character => character == '#');
        var outputGrid = splits[1].Split('/').SelectToGrid(character => character == '#');

        enchantmentRule.Output = outputGrid;

        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                enchantmentRule.Patterns.Add(GridUtils.GetGridBitmask(patternGrid));
                patternGrid = patternGrid.RotateClockwise();
            }
            patternGrid = patternGrid.FlipHorizontally();
        }

        return enchantmentRule;
    }
}
