using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day09;

[Puzzle(2018, 9, "Marble Mania")]
public partial class Day09PuzzleSolver : IPuzzleSolver
{
    private int _playerCount = 0;
    private int _lastMarbleWorth = 0;

    public void ParseInput(string[] inputLines)
    {
        var matches = InputLineRegex().Matches(inputLines[0]);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        _playerCount = int.Parse(match.Groups[1].Value);
        _lastMarbleWorth = int.Parse(match.Groups[2].Value);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetWinningPlayerScore(_playerCount, _lastMarbleWorth);

        return new PuzzleAnswer(answer, 409832L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetWinningPlayerScore(_playerCount, _lastMarbleWorth * 100L);

        return new PuzzleAnswer(answer, 3469562780L);
    }

    private static long GetWinningPlayerScore(int playerCount, long lastMarbleWorth)
    {
        var player = 0;
        var playerScores = new long[playerCount];

        var marblesList = new LinkedList<long>();
        var currentMarble = marblesList.AddFirst(0);

        for (var marbleWorth = 1L; marbleWorth <= lastMarbleWorth; marbleWorth++)
        {
            if (marbleWorth % 23 == 0)
            {
                var toBeRemovedMarble = currentMarble.PreviousCircular(7);

                playerScores[player] += marbleWorth + toBeRemovedMarble.Value;

                currentMarble = toBeRemovedMarble.NextCircular();

                marblesList.Remove(toBeRemovedMarble);
            }
            else
            {
                currentMarble = currentMarble.NextCircular();
                currentMarble = marblesList.AddAfter(currentMarble, marbleWorth);
            }

            player = (player + 1) % playerCount;
        }

        return playerScores.Max();
    }

    [GeneratedRegex(@"(\d+) players; last marble is worth (\d+) points")]
    private static partial Regex InputLineRegex();
}