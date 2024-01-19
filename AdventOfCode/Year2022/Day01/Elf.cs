namespace AdventOfCode.Year2022.Day01;

internal class Elf
{
    public IReadOnlyCollection<int> Calories { get; private set; } = [];

    public int TotalCalories => Calories.Sum();

    public static Elf Parse(IEnumerable<string> input)
    {
        var elf = new Elf
        {
            Calories = input.Select(int.Parse)
                            .ToList()
        };

        return elf;
    }
}

