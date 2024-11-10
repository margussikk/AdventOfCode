namespace AdventOfCode.Year2023.Day20;

internal class FlipFlopModule : Module
{
    public bool State { get; private set; }

    public FlipFlopModule(string name, string[] destinationModules) : base(name, destinationModules) {}
    
    public override List<Signal> ProcessSignal(Signal signal)
    {
        if (signal.Pulse)
        {
            return [];
        }
        
        State = !State;

        return DestinationModules
            .Select(destinationModule => new Signal(Name, destinationModule, State))
            .ToList();
    }

    public override void Reset()
    {
        State = false;
    }
}
