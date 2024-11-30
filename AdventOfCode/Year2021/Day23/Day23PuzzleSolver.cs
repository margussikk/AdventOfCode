using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day23;

[Puzzle(2021, 23, "Amphipod")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private Board? _board1;
    private Board? _board2;

    public void ParseInput(string[] inputLines)
    {
        _board1 = Board.Parse(inputLines);

        var inputLines2 = new[]
        {
            inputLines[0],
            inputLines[1],
            inputLines[2],
            "  #D#C#B#A#",
            "  #D#B#A#C#",
            inputLines[3],
            inputLines[4]
        };

        _board2 = Board.Parse(inputLines2);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        if (_board1 is null)
        {
            throw new InvalidOperationException("_board1 is null");
        }

        var answer = Dijkstra(_board1);

        return new PuzzleAnswer(answer, 18051);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        if (_board2 is null)
        {
            throw new InvalidOperationException("_board2 is null");
        }

        var answer = Dijkstra(_board2);

        return new PuzzleAnswer(answer, 50245);
    }

    private static int Dijkstra(Board startBoard)
    {
        var energies = new Dictionary<int, int>();

        var queue = new PriorityQueue<Board, int>();
        queue.Enqueue(startBoard, 0);

        while (queue.TryDequeue(out var board, out _))
        {
            if (board.IsSolved())
            {
                //board.PrintSteps();
                return board.Energy;
            }

            var hashCode = board.GetStatesHashCode();
            if (energies.ContainsKey(hashCode))
            {
                continue;
            }

            energies[hashCode] = board.Energy;

            var nextBoards = board.GetNextBoards();
            foreach (var nextBoard in nextBoards)
            {
                var newHashCode = nextBoard.GetStatesHashCode();
                var currentEnergy = energies.GetValueOrDefault(newHashCode, int.MaxValue);
                if (nextBoard.Energy < currentEnergy)
                {
                    queue.Enqueue(nextBoard, nextBoard.Energy);
                }
            }
        }

        return 0;
    }
}