using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2020.Day13;

[Puzzle(2020, 13, "Shuttle Search")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private int _timestamp;
    private List<IndexedBusId> _indexedBusIds = [];

    public void ParseInput(string[] inputLines)
    {
        _timestamp = int.Parse(inputLines[0]);
        _indexedBusIds = inputLines[1].Split(',')
                                      .Select(IndexedBusId.Parse)
                                      .Where(x => x.BusId != 0)
                                      .ToList();

    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var earliestBusId = 0;
        var leastWaitingTime = int.MaxValue;

        foreach(var busId in _indexedBusIds.Select(x => x.BusId))
        {
            var mod = _timestamp % busId;
            var waitingTime = mod == 0 ? 0 : busId - mod;

            if (waitingTime < leastWaitingTime)
            {
                earliestBusId = busId;
                leastWaitingTime = waitingTime;
            }
        }

        var answer = earliestBusId * leastWaitingTime;

        return new PuzzleAnswer(answer, 5257);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var congruences = _indexedBusIds
            .Select(indexedBusId => new Congruence(indexedBusId.BusId, indexedBusId.BusId - indexedBusId.Index))
            .ToList();

        var answer = ChineseRemainderTheoremSolver.Solve(congruences);

        return new PuzzleAnswer(answer, 538703333547789L);
    }
}