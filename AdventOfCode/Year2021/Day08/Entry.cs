namespace AdventOfCode.Year2021.Day08;

internal class Entry
{
    public int[] Signals { get; private init; } = [];
    public int[] Outputs { get; private init; } = [];

    public static Entry Parse(string line)
    {
        var splits = line.Split("|");

        var entry = new Entry
        {
            Signals = splits[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(ConvertToBitMask).ToArray(),
            Outputs = splits[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(ConvertToBitMask).ToArray()
        };

        return entry;
    }

    private static int ConvertToBitMask(string letters)
    {
        return letters.Aggregate(0, (acc, next) => acc | (1 << Convert.ToInt32(next - 'a')));
    }
}
