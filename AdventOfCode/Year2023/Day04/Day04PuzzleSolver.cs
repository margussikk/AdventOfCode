using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day04;

[Puzzle(2023, 4, "Scratchcards")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Card> _cards = new List<Card>();

    public void ParseInput(string[] inputLines)
    {
        _cards = inputLines.Select(Card.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _cards
            .Select(card => card.CountMatches())
            .Where(card => card != 0)
            .Select(card => 1 << (card - 1))
            .Sum();

        return new PuzzleAnswer(answer, 15268);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cardTotals = new int[_cards.Count]; // initialized to 0s

        for (var cardIndex = 0; cardIndex < cardTotals.Length; cardIndex++)
        {
            cardTotals[cardIndex]++; // Add the original card

            var matches = _cards[cardIndex].CountMatches();
            for (var copyCardIndex = cardIndex + 1; copyCardIndex < cardIndex + 1 + matches; copyCardIndex++)
            {
                cardTotals[copyCardIndex] += cardTotals[cardIndex];
            }
        }

        var answer = cardTotals.Sum();

        return new PuzzleAnswer(answer, 6283755);
    }
}
