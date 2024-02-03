using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2020.Day16;

[Puzzle(2020, 16, "Ticket Translation")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private List<TicketFieldRule> _ticketFieldRules = [];
    private List<int> _yourTicketNumbers = [];
    private List<List<int>> _nearbyTicketNumbersList = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        // Ticket field rules
        _ticketFieldRules = chunks[0].Select(TicketFieldRule.Parse)
                                     .ToList();

        // Your ticket
        if (chunks[1][0] != "your ticket:")
        {
            throw new InvalidOperationException("Failed to parse your ticket numbers");
        }

        _yourTicketNumbers = chunks[1][1].Split(',')
                                         .Select(int.Parse)
                                         .ToList();

        // Nearby tickets
        if (chunks[2][0] != "nearby tickets:")
        {
            throw new InvalidOperationException("Failed to parse nearby ticket numbers");
        }

        _nearbyTicketNumbersList = chunks[2]
            .Skip(1)
            .Select(line => line.Split(',')
                                .Select(int.Parse)
                                .ToList())
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        foreach(var ticketNumbers in _nearbyTicketNumbersList)
        {
            foreach(var ticketNumber in ticketNumbers)
            {
                var valid = _ticketFieldRules.Exists(rule => rule.IsValid(ticketNumber));
                if (!valid)
                {
                    answer += ticketNumber;
                }
            }
        }

        return new PuzzleAnswer(answer, 29019);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Keep only valid tickets
        var validNearbyTicketNumbersList = _nearbyTicketNumbersList
            .Where(ticketNumbers => ticketNumbers.TrueForAll(number => _ticketFieldRules.Exists(rule => rule.IsValid(number))))
            .ToList();

        var possibleFields = new List<TicketFieldRule>[_ticketFieldRules.Count];
        for (var fieldIndex = 0; fieldIndex < _ticketFieldRules.Count; fieldIndex++)
        {
            possibleFields[fieldIndex] = _ticketFieldRules
                .Where(rule => validNearbyTicketNumbersList.TrueForAll(numberList => rule.IsValid(numberList[fieldIndex])))
                .ToList();
        }

        var answer = 1L;
        for (var counter = 0; counter < _ticketFieldRules.Count; counter++)
        {
            var fieldIndex = Array.FindIndex(possibleFields, x => x.Count == 1);

            var rule = possibleFields[fieldIndex][0];
            if (rule.FieldName.StartsWith("departure"))
            {
                answer *= _yourTicketNumbers[fieldIndex];
            }

            possibleFields = possibleFields
                .Select(ruleList => ruleList.Where(r => r != rule)
                                            .ToList())
                .ToArray();
        }

        return new PuzzleAnswer(answer, 517827547723L);
    }
}