namespace AdventOfCode.Year2023.Day05;

internal class Map
{
    public List<Mapping> Mappings { get; }

    public Map(List<Mapping> mappings)
    {
        Mappings = mappings;
    }

    public long GetDestinationNumber(long sourceNumber)
    {
        var mapping = Mappings.Find(x => x.SourceRange.Contains(sourceNumber));

        return mapping?.GetDestination(sourceNumber) ?? sourceNumber;
    }
}
