namespace AdventOfCode.Year2020.Day13;

internal class IndexedBusId
{
    public int Index { get; private set; }

    public int BusId { get; private set; }

    public static IndexedBusId Parse(string input, int index)
    {
        var indexedBusId = new IndexedBusId
        {
            Index = index,
            BusId = 0
        };

        if (input != "x")
        {
            indexedBusId.BusId = int.Parse(input);
        }

        return indexedBusId;
    }
}
