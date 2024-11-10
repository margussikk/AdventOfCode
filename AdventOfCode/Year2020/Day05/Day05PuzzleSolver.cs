using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day05;

[Puzzle(2020, 5, "Binary Boarding")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private List<BoardingPass> _boardingPasses = [];

    public void ParseInput(string[] inputLines)
    {
        _boardingPasses = inputLines.Select(BoardingPass.Parse)
                                    .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _boardingPasses.Max(bp => bp.SeatId);

        return new PuzzleAnswer(answer, 858);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var seatIds = _boardingPasses
            .Select(bp => bp.SeatId)
            .Order()
            .ToList();

        var answer = 0;
        for (var i = 0; i < seatIds.Count - 1; i++)
        {
            var diff = seatIds[i + 1] - seatIds[i];
            if (diff <= 1) continue;
            
            answer = seatIds[i] + 1;
            break;
        }

        return new PuzzleAnswer(answer, 557);
    }
}