using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015.Day13;

[Puzzle(2015, 13, "Knights of the Dinner Table")]
public partial class Day13PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Guest> _guests = [];

    public void ParseInput(string[] inputLines)
    {
        var guests = new Dictionary<string, Guest>();

        foreach (var inputLine in inputLines)
        {
            var matches = InputLineRegex().Matches(inputLine);
            if (matches.Count != 1)
            {
                throw new InvalidOperationException("Failed to parse input line");
            }

            var match = matches[0];

            var name = match.Groups[1].Value;
            var happinessChange = int.Parse(match.Groups[3].Value);
            if (match.Groups[2].Value == "lose")
            {
                happinessChange = -happinessChange;
            }
            var neighborName = match.Groups[4].Value;

            if (!guests.TryGetValue(name, out var guest))
            {
                guest = new Guest(name);
                guests.Add(name, guest);
            }

            guest.Happinesses.Add(new NeighborHappiness(neighborName, happinessChange));
        }

        _guests = [.. guests.Values];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(_guests);

        return new PuzzleAnswer(answer, 709);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var newGuests = _guests.Select(g => g.Clone()).ToList();

        foreach (var guest in newGuests)
        {
            guest.Happinesses.Add(new NeighborHappiness("You", 0));
        }

        var youGuest = new Guest("You");
        youGuest.Happinesses.AddRange(newGuests.Select(x => new NeighborHappiness(x.Name, 0)));
        newGuests.Add(youGuest);

        var answer = GetAnswer(newGuests);

        return new PuzzleAnswer(answer, 668);
    }

    private static int GetAnswer(IReadOnlyList<Guest> guests)
    {
        var happinessesMap = guests.ToDictionary(x => x.Name, x => x.Happinesses.ToDictionary(y => y.NeighborName, y => y.Change));

        var answer = int.MinValue;

        foreach (var seatingArrangement in happinessesMap.Keys.GetPermutations().Select(x => x.ToList()))
        {
            var totalChangeInHappiness = 0;

            for (var seat = 0; seat < seatingArrangement.Count; seat++)
            {
                var name = seatingArrangement[seat];

                var leftNeighbor = seatingArrangement[(seatingArrangement.Count + seat - 1) % seatingArrangement.Count];
                var rightNeighbor = seatingArrangement[(seat + 1) % seatingArrangement.Count];

                totalChangeInHappiness += happinessesMap[name][leftNeighbor];
                totalChangeInHappiness += happinessesMap[name][rightNeighbor];
            }

            answer = int.Max(totalChangeInHappiness, answer);
        }

        return answer;
    }

    [GeneratedRegex(@"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+)")]
    private static partial Regex InputLineRegex();
}