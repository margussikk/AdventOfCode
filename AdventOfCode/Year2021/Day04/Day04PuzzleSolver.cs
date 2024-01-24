using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2021.Day04;

[Puzzle(2021, 4, "Giant Squid")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private int[] _numbers = [];
    private List<Board> _boards = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _numbers = chunks[0][0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        _boards = chunks[1..].Select(Board.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var boards = new List<Board>(_boards);
        
        foreach (var number in _numbers)
        {
            foreach (var board in boards)
            {
                board.MarkNumber(number);
                if (board.IsWinner())
                {
                    var answer = board.SumOfUnmarkedNumbers() * number;
                    return new PuzzleAnswer(answer, 45031);
                }
            }
        }

        return new PuzzleAnswer("ERROR", "NO ERROR");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        foreach (var board in _boards)
        {
            board.Reset();
        }

        var boards = new List<Board>(_boards);

        foreach (var number in _numbers)
        {
            var winningBoards = new List<Board>();

            foreach (var board in boards)
            {
                board.MarkNumber(number);
                if (board.IsWinner())
                {
                    winningBoards.Add(board);
                }
            }

            if (winningBoards.Count != 0)
            {
                if (boards.Count == 1)
                {
                    var answer = winningBoards[0].SumOfUnmarkedNumbers() * number;
                    return new PuzzleAnswer(answer, 2568);
                }

                foreach (var winningBoard in winningBoards)
                {
                    boards.Remove(winningBoard);
                }
            }
        }

        return new PuzzleAnswer("ERROR", "NO ERROR");
    }
}