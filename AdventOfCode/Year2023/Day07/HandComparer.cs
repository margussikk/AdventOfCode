namespace AdventOfCode.Year2023.Day07;

internal class HandComparer(string cardSequence) : IComparer<Hand>
{
    private readonly string _cardSequence = cardSequence;

    public int Compare(Hand? firstHand, Hand? secondHand)
    {
        ArgumentNullException.ThrowIfNull(firstHand);
        ArgumentNullException.ThrowIfNull(secondHand);

        var compareResult = firstHand.HandStrength.CompareTo(secondHand.HandStrength);
        if (compareResult != 0)
        {
            return compareResult;
        }

        foreach (var (firstHandCard, secondHandCard) in firstHand.Cards.Zip(secondHand.Cards))
        {
            var firstHandCardStrength = _cardSequence.IndexOf(firstHandCard);
            var secondHandCardStrength = _cardSequence.IndexOf(secondHandCard);

            compareResult = firstHandCardStrength.CompareTo(secondHandCardStrength);
            if (compareResult != 0)
            {
                return compareResult;
            }
        }

        return 0;
    }
}
