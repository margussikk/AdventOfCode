using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using System.Text;

namespace AdventOfCode.Year2016.Day22;

[Puzzle(2016, 22, "Grid Computing")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Node> _nodes = [];

    public void ParseInput(string[] inputLines)
    {
        _nodes = [.. inputLines.Skip(2)
                               .Select(Node.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        for (var i = 0; i < _nodes.Count; i++)
        {
            if (_nodes[i].Used == 0)
            {
                continue;
            }

            for (var j = 0; j < _nodes.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                if (_nodes[i].Used <= _nodes[j].Avail)
                {
                    answer++;
                }
            }
        }

        return new PuzzleAnswer(answer, 1007);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var maxRow = _nodes.Max(x => x.Coordinate.Row);
        var maxColumn = _nodes.Max(x => x.Coordinate.Column);

        var largeNodes = _nodes.Where(x => x.Size >= 100)
                               .Select(x => x.Coordinate)
                               .ToHashSet();

        var dataCoordinate = new GridCoordinate(0, maxColumn);
        var emptyCoordinate = _nodes.First(n => n.Used == 0).Coordinate;

        var state = new State(dataCoordinate, emptyCoordinate, 0);
        var visited = new HashSet<(GridCoordinate, GridCoordinate)>
        {
            (state.DataCoordinate, state.EmptyCoordinate)
        };

        var queue = new Queue<State>();
        queue.Enqueue(state);

        var answer = 0;

        while (queue.TryDequeue(out state))
        {
            if (state.DataCoordinate == GridCoordinate.Zero)
            {
                answer = state.Steps;
                break;
            }

            var neighborCoordinates = state.EmptyCoordinate.SideNeighbors().Where(coordinate =>
                coordinate.Row >= 0 && coordinate.Row <= maxRow &&
                coordinate.Column >= 0 && coordinate.Column <= maxColumn &&
                !largeNodes.Contains(coordinate));

            foreach (var neighborCoordinate in neighborCoordinates)
            {
                var newDataCoordinate = neighborCoordinate == state.DataCoordinate
                    ? state.EmptyCoordinate
                    : state.DataCoordinate;

                var newState = new State(newDataCoordinate, neighborCoordinate, state.Steps + 1);

                if (visited.Add((newState.DataCoordinate, newState.EmptyCoordinate)))
                {
                    queue.Enqueue(newState);
                }
            }
        }

        return new PuzzleAnswer(answer, 242);
    }

    private sealed record State(GridCoordinate DataCoordinate, GridCoordinate EmptyCoordinate, int Steps);
}