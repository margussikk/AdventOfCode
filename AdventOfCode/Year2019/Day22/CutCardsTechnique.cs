
namespace AdventOfCode.Year2019.Day22;

internal class CutCardsTechnique(int amount) : Technique
{
    public int Amount { get; } = amount;

    public override List<int> ApplyFull(List<int> deck)
    {
        if (Amount > 0)
        {
            return deck.Skip(Amount)
                       .Concat(deck.Take(Amount))
                       .ToList();
        }
        else
        {
            return deck.Skip(deck.Count + Amount)
                       .Concat(deck.Take(deck.Count + Amount))
                       .ToList();
        }
    }

    public override long Apply(long index, long deckSize)
    {
        return (index - Amount + deckSize) % deckSize;
    }

    public override long ApplyInverse(long index, long deckSize)
    {
        return (index + Amount + deckSize) % deckSize;
    }
}
