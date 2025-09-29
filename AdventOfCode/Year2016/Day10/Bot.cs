namespace AdventOfCode.Year2016.Day10;
internal class Bot
{
    public int Id { get; private set; }
    public Target LowTarget { get; private set; } = new Target();
    public Target HighTarget { get; private set; } = new Target();

    public List<int> Chips { get; } = [];
    public List<int> ComparedChips { get; private set; } = [];

    public Bot CleanCopy()
    {
        return new Bot
        {
            Id = Id,
            LowTarget = LowTarget,
            HighTarget = HighTarget
        };
    }

    public GiveInstruction[] Give(int chip)
    {
        Chips.Add(chip);
        if (Chips.Count < 2)
        {
            return [];
        }

        Chips.Sort();
        var low = Chips[0];
        var high = Chips[1];

        ComparedChips = [low, high];

        Chips.Clear();
        return [
            new GiveInstruction
            {
                Target = LowTarget,
                ChipValue = low
            },
            new GiveInstruction
            {
                Target = HighTarget,
                ChipValue = high
            }
        ];
    }

    public static Bot Parse(string line)
    {
        var parts = line.Split(' ');
        return new Bot
        {
            Id = int.Parse(parts[1]),
            LowTarget = new Target
            {
                Type = parts[5] == "bot" ? TargetType.Bot : TargetType.Output,
                Id = int.Parse(parts[6])
            },
            HighTarget = new Target
            {
                Type = parts[10] == "bot" ? TargetType.Bot : TargetType.Output,
                Id = int.Parse(parts[11])
            }
        };
    }
}
