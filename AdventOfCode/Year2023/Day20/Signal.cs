namespace AdventOfCode.Year2023.Day20;

internal class Signal
{
    public string SourceModule { get; }
    public string DestinationModule { get; }
    public bool Pulse { get; }

    public Signal(string sourceModule, string destinationModule, bool pulse)
    {
        SourceModule = sourceModule;
        DestinationModule = destinationModule;
        Pulse = pulse;
    }
}
