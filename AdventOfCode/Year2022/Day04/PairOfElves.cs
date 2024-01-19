using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2022.Day04;

internal class PairOfElves
{
    public NumberRange<int> Elf1Sections { get; private set; }
    public NumberRange<int> Elf2Sections { get; private set; }

    public static PairOfElves Parse(string line)
    {
        var splits = line.Split(',');

        var elfPair = new PairOfElves()
        {
            Elf1Sections = NumberRange<int>.Parse(splits[0]),
            Elf2Sections = NumberRange<int>.Parse(splits[1]),
        };

        return elfPair;
    }
}
