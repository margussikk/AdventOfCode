namespace AdventOfCode.Year2023.Day20;

internal abstract class Module
{
    public string Name { get; }

    public string[] DestinationModules { get; }

    protected Module(string name, string[] destinationModules)
    {
        Name = name;
        DestinationModules = destinationModules;
    }

    public abstract List<Signal> ProcessSignal(Signal signal);

    public abstract void Reset();

    public static Module Parse(string input)
    {
        var splits = input.Split(" -> ");
        var destinationModules1 = splits[1].Split(',', StringSplitOptions.TrimEntries);

        return splits[0][0] switch
        {
            '%' => new FlipFlopModule(splits[0][1..], destinationModules1),
            '&' => new ConjunctionModule(splits[0][1..], destinationModules1),
            _ => new RepeatModule(splits[0], destinationModules1)
        };
    }
}
