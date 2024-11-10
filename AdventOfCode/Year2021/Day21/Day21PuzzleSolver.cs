using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day21;

[Puzzle(2021, 21, "Dirac Dice")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private int _player1StartingPosition;
    private int _player2StartingPosition;

    public void ParseInput(string[] inputLines)
    {
        _player1StartingPosition = int.Parse(inputLines[0].Replace("Player 1 starting position: ", string.Empty));
        _player2StartingPosition = int.Parse(inputLines[1].Replace("Player 2 starting position: ", string.Empty));
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var dice = new DeterministicDice();
        var player1 = new Player(_player1StartingPosition);
        var player2 = new Player(_player2StartingPosition);
        var currentPlayer = player2;

        do
        {
            currentPlayer = currentPlayer == player2 ? player1 : player2;

            var rolledScore = dice.Roll() + dice.Roll() + dice.Roll();
            var landedOn = (currentPlayer.Position + rolledScore - 1) % 10 + 1;
            currentPlayer.Position = landedOn;
            currentPlayer.Score += landedOn;
        }
        while (currentPlayer.Score < 1000);

        var answer = Math.Min(player1.Score, player2.Score) * dice.RollCount;

        return new PuzzleAnswer(answer, 929625);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var winsMap = new Dictionary<GameState, Wins>();

        var frequencies = new int[10];
        for (var dice1 = 1; dice1 <= 3; dice1++)
        {
            for (var dice2 = 1; dice2 <= 3; dice2++)
            {
                for (var dice3 = 1; dice3 <= 3; dice3++)
                {
                    frequencies[dice1 + dice2 + dice3]++;
                }
            }
        }

        var finalWins = CountWins(_player1StartingPosition, 0, _player2StartingPosition, 0);

        var answer = Math.Max(finalWins.ActivePlayerWins, finalWins.IdlePlayerWins);

        return new PuzzleAnswer(answer, 175731756652760L);


        Wins CountWins(int activePlayerPosition, int activePlayerScore, int idlePlayerPosition, int idlePlayerScore)
        {
            if (activePlayerScore >= 21)
            {
                return new Wins { ActivePlayerWins = 1 };
            }

            if (idlePlayerScore >= 21)
            {
                return new Wins { IdlePlayerWins = 1 };
            }

            var state = new GameState(activePlayerPosition, activePlayerScore, idlePlayerPosition, idlePlayerScore);
            if (winsMap.TryGetValue(state, out var memoized))
            {
                return memoized;
            }

            var wins = new Wins();
            for (var rolledScore = 3; rolledScore <= 9; rolledScore++)
            {
                var newActivePlayerPosition = (activePlayerPosition + rolledScore - 1) % 10 + 1;
                var newActivePlayerScore = activePlayerScore + newActivePlayerPosition;

                // Switch players, because only active player rolls the dice above
                var wins2 = CountWins(idlePlayerPosition, idlePlayerScore, newActivePlayerPosition, newActivePlayerScore);

                // Since we switched players, we also need to count switched wins
                wins.ActivePlayerWins += wins2.IdlePlayerWins * frequencies[rolledScore];
                wins.IdlePlayerWins += wins2.ActivePlayerWins * frequencies[rolledScore];
            }

            winsMap[state] = wins;
            return wins;
        }
    }
}