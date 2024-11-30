namespace AdventOfCode.Year2023.Day20;

internal class RepeatModule : Module
{
    public RepeatModule(string name, string[] destinationModules) : base(name, destinationModules) { }

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
