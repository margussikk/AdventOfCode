namespace AdventOfCode.Year2016.Day10;

internal class GiveInstruction
{
    public Target Target { get; init; } = new Target();
    public int ChipValue { get; init; }

    public static GiveInstruction Parse(string line)
    {
        var parts = line.Split(' ');
        return new GiveInstruction
        {
            Target = new Target
            {
                Type = parts[4] == "bot" ? TargetType.Bot : TargetType.Output,
                Id = int.Parse(parts[5])
            },
            ChipValue = int.Parse(parts[1])
        };
    }
}
