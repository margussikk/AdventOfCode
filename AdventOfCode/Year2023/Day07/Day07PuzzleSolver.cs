using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day07;

[Puzzle(2023, 7, "Camel Cards")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private List<Hand> _hands = [];

    public void ParseInput(List<string> inputLines)
    {
        _hands = inputLines.Select(Hand.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        _hands.ForEach(hand => hand.FillHandTypeForPartOne());

        var answer = _hands.Order(new HandComparer("23456789TJQKA"))
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();

        return new PuzzleAnswer(answer, 247961593);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        _hands.ForEach(hand => hand.FillHandTypeForPartTwo());

        var answer = _hands.Order(new HandComparer("J23456789TQKA"))
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();

        return new PuzzleAnswer(answer, 248750699);
    }
}
