using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2020.Day22;

[Puzzle(2020, 22, "Crab Combat")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private List<int> _player1Cards = [];
    private List<int> _player2Cards = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        foreach (var chunk in chunks)
        {
            switch (chunk[0])
            {
                case "Player 1:":
                    _player1Cards = chunk.Skip(1)
                        .Select(int.Parse)
                        .ToList();
                    break;
                case "Player 2:":
                    _player2Cards = chunk.Skip(1)
                        .Select(int.Parse)
                        .ToList();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected player");
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var player1Deck = new Queue<int>(_player1Cards);
        var player2Deck = new Queue<int>(_player2Cards);

        while (player1Deck.Count > 0 && player2Deck.Count > 0)
        {
            var player1TopCard = player1Deck.Dequeue();
            var player2TopCard = player2Deck.Dequeue();

            if (player1TopCard > player2TopCard)
            {
                player1Deck.Enqueue(player1TopCard);
                player1Deck.Enqueue(player2TopCard);
            }
            else
            {
                player2Deck.Enqueue(player2TopCard);
                player2Deck.Enqueue(player1TopCard);
            }
        }

        var winnerDeck = player1Deck.Count > 0 ? player1Deck : player2Deck;

        var answer = CalculateScore(winnerDeck);

        return new PuzzleAnswer(answer, 32472);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var (_, winnerDeck) = PlayRecursiveCombat(_player1Cards, _player2Cards);

        var answer = CalculateScore(winnerDeck);

        return new PuzzleAnswer(answer, 36463);
    }

    private static (int Winner, Queue<int> WinnerDeck) PlayRecursiveCombat(IEnumerable<int> player1Cards, IEnumerable<int> player2Cards)
    {
        var gameStates = new HashSet<GameState>();

        var player1Deck = new Queue<int>(player1Cards);
        var player2Deck = new Queue<int>(player2Cards);

        while (player1Deck.Count > 0 && player2Deck.Count > 0)
        {
            var gameState = new GameState(CalculateScore(new Queue<int>(player1Deck)), CalculateScore(new Queue<int>(player2Deck)));
            if (!gameStates.Add(gameState))
            {
                return (1, player1Deck);
            }

            var player1TopCard = player1Deck.Dequeue();
            var player2TopCard = player2Deck.Dequeue();

            if (player1Deck.Count >= player1TopCard &&
                player2Deck.Count >= player2TopCard)
            {
                // Subgame
                var (winner, _) = PlayRecursiveCombat(player1Deck.Take(player1TopCard), player2Deck.Take(player2TopCard));
                if (winner == 1)
                {
                    player1Deck.Enqueue(player1TopCard);
                    player1Deck.Enqueue(player2TopCard);
                }
                else
                {
                    player2Deck.Enqueue(player2TopCard);
                    player2Deck.Enqueue(player1TopCard);
                }
            }
            else if (player1TopCard > player2TopCard)
            {
                player1Deck.Enqueue(player1TopCard);
                player1Deck.Enqueue(player2TopCard);
            }
            else
            {
                player2Deck.Enqueue(player2TopCard);
                player2Deck.Enqueue(player1TopCard);
            }
        }

        return player1Deck.Count > 0
            ? (1, player1Deck)
            : (2, player2Deck);
    }

    private static int CalculateScore(Queue<int> deck)
    {
        var score = 0;

        while (deck.TryDequeue(out var topCard))
        {
            score += topCard * (deck.Count + 1);
        }

        return score;
    }

    private record struct GameState(int Player1Score, int Player2Score);
}