namespace AdventOfCode.Year2023.Day20;

internal class ConjunctionModule(string name, string[] destinationModules) : Module(name, destinationModules)
{
    public Dictionary<string, bool> InputPulses { get; private set; } = [];

    public void InitInputPulses(string[] inputModules)
    {
        InputPulses = inputModules.ToDictionary(x => x, x => false);
    }

    public override List<Signal> ProcessSignal(Signal signal)
    {
        InputPulses[signal.SourceModule] = signal.Pulse;

        var outputPulse = !InputPulses.All(x => x.Value);

        return DestinationModules
            .Select(destinationModule => new Signal(Name, destinationModule, outputPulse))
            .ToList();
    }

    public override void Reset()
    {
        foreach (var module in InputPulses.Keys)
        {
            InputPulses[module] = false;
        }
    }
}
