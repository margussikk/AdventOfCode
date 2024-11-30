
namespace AdventOfCode.Year2019.Day22;

internal class DealWithIncrementTechnique : Technique
{
    public int Increment { get; }

    public DealWithIncrementTechnique(int increment)
    {
        Increment = increment;
    }

    public override List<int> ApplyFull(List<int> deck)
    {
        var newDeck = new int[deck.Count];

        var index = 0;

        foreach (var card in deck)
        {
            newDeck[index] = card;

            index = (index + Increment) % deck.Count;
        }

        return [.. newDeck];
    }

    public override long Apply(long index, long deckSize)
    {
        return index * Increment % deckSize;
    }

    public override long ApplyInverse(long index, long deckSize)
    {
        var deckCount = 0;

        while ((deckCount * deckSize + index) % Increment != 0)
        {
            deckCount++;
        }

        return (deckCount * deckSize + index) / Increment;
    }
}
