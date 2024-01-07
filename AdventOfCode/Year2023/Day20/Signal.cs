namespace AdventOfCode.Year2023.Day20;

internal class Signal(string sourceModule, string destinationModule, bool pulse)
{
    public string SourceModule { get; } = sourceModule;
    public string DestinationModule { get; } = destinationModule;
    public bool Pulse { get; } = pulse;
}
