namespace AdventOfCode.Year2023.Day20;

internal class FlipFlopModule(string name, string[] destinationModules) : Module(name, destinationModules)
{
    public bool State { get; private set; }

    public override List<Signal> ProcessSignal(Signal signal)
    {
        if (signal.Pulse)
        {
            return [];
        }
        else
        {
            State = !State;

            return DestinationModules
                .Select(destinationModule => new Signal(Name, destinationModule, State))
                .ToList();
        }
    }

    public override void Reset()
    {
        State = false;
    }
}
