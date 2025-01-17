using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Graph;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2019.Day18;

[Puzzle(2019, 18, "Many-Worlds Interpretation")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _part1Grid = new(0, 0);

    private readonly Dictionary<GridCoordinate, char> _keys = [];
    private readonly Dictionary<GridCoordinate, char> _doors = [];
    private readonly Dictionary<GridCoordinate, char> _part1Entrances = [];

    public void ParseInput(string[] inputLines)
    {
        _part1Grid = inputLines.SelectToGrid<Tile>((character, coordinate) =>
        {
            var tile = character switch
            {
                '.' => Tile.Space,
                '#' => Tile.Wall,
                >= 'a' and <= 'z' => Tile.Key,
                >= 'A' and <= 'Z' => Tile.Door,
                '@' => Tile.Entrance,
                _ => throw new InvalidOperationException("Invalid character")
            };

            switch (tile)
            {
                case Tile.Entrance:
                    _part1Entrances[coordinate] = character;
                    break;
                case Tile.Key:
                    _keys[coordinate] = character;
                    break;
                case Tile.Door:
                    _doors[coordinate] = character;
                    break;
            }

            return tile;
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = CountFewestSteps(_part1Grid, _part1Entrances);

        return new PuzzleAnswer(answer, 4544);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var part2Grid = _part1Grid.Clone();

        var entranceCoordinate = part2Grid.FindCoordinate(tile => tile == Tile.Entrance)
            ?? throw new InvalidOperationException("Entrance tile not found");

        part2Grid[entranceCoordinate.Move(GridDirection.UpLeft)] = Tile.Entrance;
        part2Grid[entranceCoordinate.Up()] = Tile.Wall;
        part2Grid[entranceCoordinate.Move(GridDirection.UpRight)] = Tile.Entrance;

        part2Grid[entranceCoordinate.Left()] = Tile.Wall;
        part2Grid[entranceCoordinate] = Tile.Wall;
        part2Grid[entranceCoordinate.Right()] = Tile.Wall;

        part2Grid[entranceCoordinate.Move(GridDirection.DownLeft)] = Tile.Entrance;
        part2Grid[entranceCoordinate.Down()] = Tile.Wall;
        part2Grid[entranceCoordinate.Move(GridDirection.DownRight)] = Tile.Entrance;

        var part2Entrances = new Dictionary<GridCoordinate, char>
        {
            [entranceCoordinate.Move(GridDirection.UpLeft)] = '@',
            [entranceCoordinate.Move(GridDirection.UpRight)] = '$',
            [entranceCoordinate.Move(GridDirection.DownLeft)] = '~',
            [entranceCoordinate.Move(GridDirection.DownRight)] = '%'
        };

        var answer = CountFewestSteps(part2Grid, part2Entrances);

        return new PuzzleAnswer(answer, 1692);
    }


    private GraphBuilder BuildGraphBuilder(Grid<Tile> grid, Dictionary<GridCoordinate, char> entrances)
    {
        var graphBuilder = new GraphBuilder();

        List<GridCoordinate> startCoordinates = [
            .. entrances.Keys,
            .. _keys.Keys,
            .. _doors.Keys
        ];

        foreach (var startCoordinate in startCoordinates)
        {
            var gridWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);

            var queue = new Queue<GridWalker>();
            queue.Enqueue(gridWalker);

            var visited = new HashSet<GridCoordinate>();

            while (queue.TryDequeue(out gridWalker))
            {
                if (!visited.Add(gridWalker.Coordinate)) continue;

                var tile = grid[gridWalker.Coordinate];

                if (gridWalker.StartCoordinate != gridWalker.Coordinate && tile is Tile.Key or Tile.Door)
                {
                    var sourceTile = grid[gridWalker.StartCoordinate];
                    var sourceName = sourceTile switch
                    {
                        Tile.Entrance => entrances[gridWalker.StartCoordinate].ToString(),
                        Tile.Key => _keys[gridWalker.StartCoordinate].ToString(),
                        Tile.Door => _doors[gridWalker.StartCoordinate].ToString(),
                        _ => throw new InvalidOperationException("Invalid source tile")
                    };

                    var destinationTile = grid[gridWalker.Coordinate];
                    var destinationName = destinationTile switch
                    {
                        Tile.Key => _keys[gridWalker.Coordinate].ToString(),
                        Tile.Door => _doors[gridWalker.Coordinate].ToString(),
                        _ => throw new InvalidOperationException("Invalid destination tile")
                    };

                    graphBuilder.AddConnection(sourceName, GraphVertexPort.Any, destinationName, GraphVertexPort.Any, gridWalker.Steps);

                    var sourceVertex = graphBuilder.Vertices[sourceName];
                    sourceVertex.Object ??= new VertexObject(sourceName, sourceTile);

                    var destinationVertex = graphBuilder.Vertices[destinationName];
                    destinationVertex.Object ??= new VertexObject(destinationName, destinationTile);
                }
                else
                {
                    var neighbors = grid.SideNeighbors(gridWalker.Coordinate)
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

        return graphBuilder;
    }

    private int CountFewestSteps(Grid<Tile> grid, Dictionary<GridCoordinate, char> entrances)
    {
        var graphBuilder = BuildGraphBuilder(grid, entrances);

        var visited = new HashSet<(int, long)>();

        var queue = new PriorityQueue<VertexWalker, int>();

        var allKeys = graphBuilder.Vertices.Values
            .Select(x => x.Object)
            .Cast<VertexObject>()
            .Where(x => x.Tile == Tile.Key)
            .Select(x => x.KeyBitMask)
            .Sum();

        var startVertices = graphBuilder.Vertices.Values
            .Where(x => x.Object != null && ((VertexObject)x.Object).Tile == Tile.Entrance)
            .ToArray();

        var walker = new VertexWalker(startVertices);
        queue.Enqueue(walker, 0);

        while (queue.TryDequeue(out walker, out _))
        {
            var stateIds = walker.CurrentVertices.Aggregate(0, (current, vertex) => current * 100 + vertex.Id);

            var state = (stateIds, walker.Keys);
            if (!visited.Add(state)) continue;

            foreach (var vertex in walker.CurrentVertices)
            {
                if (vertex.Object is VertexObject { Tile: Tile.Key } currentVertexObject)
                {
                    walker.Keys |= currentVertexObject.KeyBitMask;
                }
            }

            if (walker.Keys == allKeys)
            {
                return walker.Steps;
            }

            for (var index = 0; index < walker.CurrentVertices.Length; index++)
            {
                foreach (var edge in walker.CurrentVertices[index].Edges.Where(e => e.SourceVertex == walker.CurrentVertices[index]))
                {
                    if (edge.DestinationVertex.Object is not VertexObject destinationObject ||
                        (destinationObject.Tile != Tile.Key &&
                         (destinationObject.Tile != Tile.Door ||
                          (walker.Keys & destinationObject.KeyBitMask) == 0))) continue;

                    GraphVertex[] newVertices = [.. walker.CurrentVertices];

                    newVertices[index] = edge.DestinationVertex;

                    var nextWalker = new VertexWalker(newVertices)
                    {
                        Steps = walker.Steps + edge.Weight,
                        Keys = walker.Keys
                    };

                    queue.Enqueue(nextWalker, nextWalker.Steps);
                }
            }
        }

        return int.MaxValue;
    }
}