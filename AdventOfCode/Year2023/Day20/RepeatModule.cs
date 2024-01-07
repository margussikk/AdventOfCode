namespace AdventOfCode.Year2023.Day20;

internal class RepeatModule(string name, string[] destinationModules) : Module(name, destinationModules)
{
    public override List<Signal> ProcessSignal(Signal signal)
    {
        return DestinationModules
            .Select(destinationModule => new Signal(Name, destinationModule, signal.Pulse))
            .ToList();
    }

    public override void Reset()
    {
        // Do nothing
    }
}
