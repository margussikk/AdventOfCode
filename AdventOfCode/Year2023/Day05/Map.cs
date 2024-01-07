namespace AdventOfCode.Year2023.Day05;

internal class Map(List<Mapping> mappings)
{
    public List<Mapping> Mappings { get; } = mappings;

    public long GetDestinationNumber(long sourceNumber)
    {
        var mapping = Mappings.Find(x => x.SourceRange.Contains(sourceNumber));

        return mapping != null
            ? mapping.GetDestination(sourceNumber)
            : sourceNumber;
    }
}
