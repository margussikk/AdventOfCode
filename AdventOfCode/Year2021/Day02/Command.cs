namespace AdventOfCode.Year2021.Day02;

internal class Command
{
    public Direction Direction { get; private set; }
    public int Units { get; private set; }

    public static Command Parse(string input)
    {
        var splits = input.Split(" ");

        return new Command
        {
            Direction = Enum.Parse<Direction>(splits[0], true),
            Units = int.Parse(splits[1])
        };
    }
}