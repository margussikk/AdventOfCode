using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Graph;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day24;

[Puzzle(2016, 24, "Air Duct Spelunking")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<GridCoordinate, int> _coordinateNumbers = [];
    private Grid<Tile> _grid = new(0, 0);
    private readonly GraphBuilder _graphBuilder = new();

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid((character, coordinate) =>
        {
            var tile = character switch
            {
                '.' => Tile.OpenSpace,
                '#' => Tile.Wall,
                _ => Tile.Number
            };

            if (tile == Tile.Number)
            {
                _coordinateNumbers[coordinate] = character.ParseToDigit();
            }

            return tile;
        });

        BuildGraphBuilder();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var allNumbers = _graphBuilder.Vertices.Sum(x => 1 << x.Value.Id);

        var startVertex = _graphBuilder.Vertices["0"];

        var walker = new VertexWalker(startVertex)
        {
            Numbers = 1 << startVertex.Id,
        };
        var queue = new PriorityQueue<VertexWalker, int>();
        queue.Enqueue(walker, 0);

        var answer = 0;

        while (queue.TryDequeue(out walker, out _))
        {
            if (walker.Numbers == allNumbers)
            {
                answer = walker.Steps;
                break;
            }

            foreach (var edge in walker.CurrentVertex.Edges.Where(e => e.SourceVertex == walker.CurrentVertex))
            {
                var numberBitMask = 1 << edge.DestinationVertex.Id;

                if ((walker.Numbers & numberBitMask) == numberBitMask) continue;

                var nextWalker = new VertexWalker(edge.DestinationVertex)
                {
                    Steps = walker.Steps + edge.Weight,
                    Numbers = walker.Numbers | numberBitMask
                };

                queue.Enqueue(nextWalker, nextWalker.Steps);
            }
        }

        return new PuzzleAnswer(answer, 518);
    }

    // This solution assumes that every number is reachable from number 0 and we don't have to passthrough other numbers
    public PuzzleAnswer GetPartTwoAnswer()
    {
        var allNumbers = _graphBuilder.Vertices.Sum(x => 1 << x.Value.Id);

        var startVertex = _graphBuilder.Vertices["0"];

        var walker = new VertexWalker(startVertex)
        {
            Numbers = 1 << startVertex.Id
        };

        var queue = new PriorityQueue<VertexWalker, int>();
        queue.Enqueue(walker, 0);

        var answer = int.MaxValue / 2;

        while (queue.TryDequeue(out walker, out _))
        {
            var stepsBackToStart = walker.CurrentVertex.Edges.FirstOrDefault(x => x.DestinationVertex.Name == "0")?.Weight ?? 0;

            if (walker.Numbers == allNumbers)
            {
                answer = int.Min(answer, walker.Steps + stepsBackToStart);
                continue;
            }

            if (walker.Steps + stepsBackToStart > answer)
            {
                continue;
            }

            foreach (var edge in walker.CurrentVertex.Edges.Where(e => e.SourceVertex == walker.CurrentVertex))
            {
                var numberBitMask = 1 << edge.DestinationVertex.Id;

                if ((walker.Numbers & numberBitMask) == numberBitMask) continue;

                var nextWalker = new VertexWalker(edge.DestinationVertex)
                {
                    Steps = walker.Steps + edge.Weight,
                    Numbers = walker.Numbers | numberBitMask
                };

                queue.Enqueue(nextWalker, nextWalker.Steps);
            }
        }

        return new PuzzleAnswer(answer, 716);
    }

    private void BuildGraphBuilder()
    {
        foreach (var startCoordinate in _coordinateNumbers.Keys)
        {
            var gridWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);

            var queue = new Queue<GridWalker>();
            queue.Enqueue(gridWalker);

            var visited = new HashSet<GridCoordinate>();

            while (queue.TryDequeue(out gridWalker))
            {
                if (!visited.Add(gridWalker.Coordinate)) continue;

                var tile = _grid[gridWalker.Coordinate];

                if (gridWalker.StartCoordinate != gridWalker.Coordinate && tile is Tile.Number)
                {
                    var sourceName = _coordinateNumbers[gridWalker.StartCoordinate].ToString();
                    var destinationName = _coordinateNumbers[gridWalker.Coordinate].ToString();

                    _graphBuilder.AddConnection(sourceName, GraphVertexPort.Any, destinationName, GraphVertexPort.Any, gridWalker.Steps);
                }
                else
                {
                    var neighbors = _grid.SideNeighbors(gridWalker.Coordinate)
                                         .Where(cell => cell.Object != Tile.Wall);

                    foreach (var neighbor in neighbors)
                    {
                        var newGridWalker = gridWalker.Clone();

                        var direction = gridWalker.Coordinate.DirectionToward(neighbor.Coordinate);
                        newGridWalker.Move(direction);

                        queue.Enqueue(newGridWalker);
                    }
                }
            }
        }
    }
}