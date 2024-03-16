
namespace AdventOfCode.Year2019.Day22;

internal class DealIntoNewStackTechnique : Technique
{
    public override List<int> ApplyFull(List<int> deck)
    {
        return deck.Reverse<int>().ToList();
    }

    public override long Apply(long index, long deckSize)
    {
        return deckSize - 1 - index;
    }

    public override long ApplyInverse(long index, long deckSize)
    {
        return deckSize - 1 - index;
    }
}
