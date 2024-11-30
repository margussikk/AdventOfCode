using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2023.Day23;

[Puzzle(2023, 23, "A Long Walk")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private GraphVertex? _startVertex;

    private GraphVertex? _endVertex;

    public void ParseInput(string[] inputLines)
    {
        var tiles = inputLines.SelectToGrid(character => character switch
        {
            '.' => Tile.Path,
            '#' => Tile.Forest,
            '^' => Tile.SlopeUp,
            'v' => Tile.SlopeDown,
            '<' => Tile.SlopeLeft,
            '>' => Tile.SlopeRight,
            _ => throw new InvalidOperationException()
        });

        // Start
        GridCoordinate? startCoordinate = null;
        for (var column = 0; column < tiles.Width; column++)
        {
            if (tiles[0, column] != Tile.Path) continue;

            startCoordinate = new GridCoordinate(0, column);
            break;
        }

        if (startCoordinate is null)
        {
            throw new InvalidOperationException("Failed to find start");
        }

        // End
        GridCoordinate? endCoordinate = null;
        for (var column = 0; column < tiles.Width; column++)
        {
            if (tiles[tiles.LastRowIndex, column] != Tile.Path) continue;

            endCoordinate = new GridCoordinate(tiles.LastRowIndex, column);
            break;
        }

        if (endCoordinate is null)
        {
            throw new InvalidOperationException("Failed to find end");
        }

        // Build graph
        var graphBuilder = BuildGraph(tiles, startCoordinate.Value, endCoordinate.Value);

        _startVertex = graphBuilder.Vertices[startCoordinate.Value.ToString()];
        _endVertex = graphBuilder.Vertices[endCoordinate.Value.ToString()];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetLongestPath(false);

        return new PuzzleAnswer(answer, 2170);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetLongestPath(true);

        return new PuzzleAnswer(answer, 6502);
    }

    private int GetLongestPath(bool ignoreSlopes)
    {
        var longestPath = 0;

        if (_startVertex == null)
        {
            throw new InvalidOperationException("Start vertex is null");
        }

        if (_endVertex == null)
        {
            throw new InvalidOperationException("End vertex is null");
        }

        var vertexHikers = new Stack<VertexHiker>();

        var vertexHiker = new VertexHiker(_startVertex, 0, 0);
        vertexHikers.Push(vertexHiker);

        while (vertexHikers.TryPop(out vertexHiker))
        {
            if (vertexHiker.CurrentVertex == _endVertex)
            {
                longestPath = int.Max(longestPath, vertexHiker.Distance);
                continue;
            }

            var bitmask = 1L << vertexHiker.CurrentVertex.Id;
            if ((vertexHiker.VisitedBitMask & bitmask) != 0)
            {
                continue;
            }

            vertexHiker.VisitedBitMask |= bitmask;

            foreach (var edge in vertexHiker.CurrentVertex.Edges.Where(e => e.SourceVertex == vertexHiker.CurrentVertex || ignoreSlopes))
            {
                var destinationVertex = edge.SourceVertex == vertexHiker.CurrentVertex ? edge.DestinationVertex : edge.SourceVertex;

                var newVertexHiker = new VertexHiker(destinationVertex, vertexHiker.Distance + edge.Weight, vertexHiker.VisitedBitMask);
                vertexHikers.Push(newVertexHiker);
            }
        }

        return longestPath;
    }

    private static GraphBuilder BuildGraph(Grid<Tile> tiles, GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        var hikeGraphBuilder = new GraphBuilder();

        var vertexCoordinates = tiles
            .Where(cell => cell.Coordinate == startCoordinate ||
                        cell.Coordinate == endCoordinate ||
                        (cell.Object != Tile.Forest && tiles.SideNeighbors(cell.Coordinate).Count(c => c.Object != Tile.Forest) > 2))
            .Select(x => x.Coordinate)
            .ToHashSet();

        var visitedGrid = new BitGrid(tiles.Height, tiles.Width);

        // Start walking from every vertex coordinate
        foreach (var vertexCoordinate in vertexCoordinates)
        {
            var hikers = new Stack<GridWalker>();

            // Add neighbors
            foreach (var neighborCell in tiles.SideNeighbors(vertexCoordinate).Where(cell => cell.Object != Tile.Forest))
            {
                var direction = vertexCoordinate.DirectionToward(neighborCell.Coordinate);

                var newHiker = new GridWalker(vertexCoordinate, vertexCoordinate, direction, 0);
                hikers.Push(newHiker);
            }

            while (hikers.TryPop(out var hiker))
            {
                if (vertexCoordinates.Contains(hiker.CurrentCoordinate) && hiker.CurrentCoordinate != hiker.StartCoordinate)
                {
                    var entranceTileType = tiles[hiker.CurrentCoordinate.Move(hiker.Direction.Flip())];

                    if (entranceTileType is Tile.Path ||
                        (hiker.Direction == GridDirection.Down && entranceTileType is Tile.SlopeDown) ||
                        (hiker.Direction == GridDirection.Up && entranceTileType is Tile.SlopeUp) ||
                        (hiker.Direction == GridDirection.Left && entranceTileType is Tile.SlopeLeft) ||
                        (hiker.Direction == GridDirection.Right && entranceTileType is Tile.SlopeRight))
                    {
                        // Source and destination are in correct order.
                        hikeGraphBuilder.AddConnection(hiker.StartCoordinate.ToString(), GraphVertexPort.Any, hiker.CurrentCoordinate.ToString(), GraphVertexPort.Any, hiker.Steps);
                    }
                    else
                    {
                        // Source and destination are reversed. Swap coordinates.
                        hikeGraphBuilder.AddConnection(hiker.CurrentCoordinate.ToString(), GraphVertexPort.Any, hiker.StartCoordinate.ToString(), GraphVertexPort.Any, hiker.Steps);
                    }

                    foreach (var neighborCell in tiles.SideNeighbors(hiker.CurrentCoordinate).Where(c => c.Object != Tile.Forest))
                    {
                        var direction = hiker.CurrentCoordinate.DirectionToward(neighborCell.Coordinate);
                        if (direction == hiker.Direction.Flip()) continue;

                        var newHiker = new GridWalker(hiker.CurrentCoordinate, neighborCell.Coordinate, direction, 1);
                        hikers.Push(newHiker);
                    }

                    visitedGrid[hiker.CurrentCoordinate] = true;
                    continue;
                }

                if (visitedGrid[hiker.CurrentCoordinate])
                {
                    continue;
                }

                visitedGrid[hiker.CurrentCoordinate] = true;

                // Find neighbors
                foreach (var neighborCell in tiles.SideNeighbors(hiker.CurrentCoordinate).Where(c => c.Object != Tile.Forest))
                {
                    var direction = hiker.CurrentCoordinate.DirectionToward(neighborCell.Coordinate);
                    if (direction == hiker.Direction.Flip()) continue;

                    var newHiker = hiker.Clone();
                    newHiker.Move(direction);

                    hikers.Push(newHiker);
                }
            }
        }

        return hikeGraphBuilder;
    }
}
