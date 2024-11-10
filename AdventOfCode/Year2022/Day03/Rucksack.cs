namespace AdventOfCode.Year2022.Day03;

internal class Rucksack
{
    public string Compartment1Items { get; private set; } = string.Empty;

    public string Compartment2Items { get; private set; } = string.Empty;

    public static Rucksack Parse(string line)
    {
        var itemsCount = line.Length / 2;

        var rucksack = new Rucksack
        {
            Compartment1Items = line[..itemsCount],
            Compartment2Items = line[itemsCount..]
        };

        return rucksack;
    }
}
