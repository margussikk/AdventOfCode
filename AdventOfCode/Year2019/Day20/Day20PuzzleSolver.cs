using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day20;

[Puzzle(2019, 20, "Donut Maze")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<string, List<GridCoordinate>> _portalCoordinates = [];
    private Grid<Tile> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = new Grid<Tile>(inputLines.Length, inputLines[0].Length);

        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                var character = inputLines[row][column];

                switch (character)
                {
                    case ' ':
                        _grid[row, column] = Tile.Empty;
                        break;
                    case '.':
                        _grid[row, column] = Tile.Passage;
                        break;
                    case '#':
                        _grid[row, column] = Tile.Wall;
                        break;
                    case >= 'A' and <= 'Z':
                    {
                        var portalName = string.Empty;

                        // Portal
                        if (_grid.InBounds(row, column - 1) && inputLines[row][column - 1] == '.') // Right of passage tile
                        {
                            portalName = $"{inputLines[row][column]}{inputLines[row][column + 1]}";
                        }
                        else if (_grid.InBounds(row - 1, column) && inputLines[row - 1][column] == '.') // Below passage tile
                        {
                            portalName = $"{inputLines[row][column]}{inputLines[row + 1][column]}";
                        }
                        else if (_grid.InBounds(row, column + 1) && inputLines[row][column + 1] == '.') // Left of passage tile
                        {
                            portalName = $"{inputLines[row][column - 1]}{inputLines[row][column]}";
                        }
                        else if (_grid.InBounds(row + 1, column) && inputLines[row + 1][column] == '.') // Above passage tile
                        {
                            portalName = $"{inputLines[row - 1][column]}{inputLines[row][column]}";
                        }

                        if (!string.IsNullOrEmpty(portalName))
                        {
                            if (!_portalCoordinates.TryGetValue(portalName, out var coordinates))
                            {
                                coordinates = [];
                                _portalCoordinates[portalName] = coordinates;
                            }

                            coordinates.Add(new GridCoordinate(row, column));
                            _grid[row, column] = Tile.Portal;
                        }
                        else
                        {
                            _grid[row, column] = Tile.Empty;
                        }

                        break;
                    }
                    default:
                        throw new InvalidOperationException($"Invalid tile character {character}");
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        var startCoordinate = _grid
            .SideNeighbors(_portalCoordinates["AA"][0])
            .First(cell => cell.Object == Tile.Passage)
            .Coordinate;

        var endCoordinate = _grid
            .SideNeighbors(_portalCoordinates["ZZ"][0])
            .First(cell => cell.Object == Tile.Passage)
            .Coordinate;

        var visited = new BitGrid(_grid.Height, _grid.Width);

        var queue = new PriorityQueue<Part1MazeWalker, int>();

        var mazeWalker = new Part1MazeWalker(startCoordinate, 0);
        queue.Enqueue(mazeWalker, mazeWalker.Steps);

        while(queue.TryDequeue(out mazeWalker, out _))
        {
            if (visited[mazeWalker.Coordinate])
            {
                continue;
            }

            visited[mazeWalker.Coordinate] = true;
            
            if (mazeWalker.Coordinate == endCoordinate)
            {
                answer = mazeWalker.Steps;
                break;
            }

            foreach (var neighbor in _grid.SideNeighbors(mazeWalker.Coordinate))
            {
                switch (neighbor.Object)
                {
                    case Tile.Passage:
                    {
                        var newMazeWalker = new Part1MazeWalker(neighbor.Coordinate, mazeWalker.Steps + 1);
                        queue.Enqueue(newMazeWalker, newMazeWalker.Steps);
                        break;
                    }
                    case Tile.Portal:
                    {
                        var exitPortalCoordinate = _portalCoordinates.Values
                            .First(x => x.Contains(neighbor.Coordinate))
                            .Cast<GridCoordinate?>()
                            .FirstOrDefault(x => x != neighbor.Coordinate);

                        if (exitPortalCoordinate is not null)
                        {
                            var exitCoordinate = _grid
                                .SideNeighbors(exitPortalCoordinate.Value)
                                .First(cell => cell.Object == Tile.Passage)
                                .Coordinate;

                            var newMazeWalker = new Part1MazeWalker(exitCoordinate, mazeWalker.Steps + 1);

                            queue.Enqueue(newMazeWalker, newMazeWalker.Steps);
                        }

                        break;
                    }
                }
            }
        }

        return new PuzzleAnswer(answer, 400);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var gridCenter = new GridCoordinate(_grid.Height / 2, _grid.Width / 2);
        var answer = 0;

        var startCoordinate = _grid
            .SideNeighbors(_portalCoordinates["AA"][0])
            .First(cell => cell.Object == Tile.Passage)
            .Coordinate;

        var endCoordinate = _grid
            .SideNeighbors(_portalCoordinates["ZZ"][0])
            .First(cell => cell.Object == Tile.Passage)
            .Coordinate;

        var visited = new List<BitGrid>();

        var queue = new PriorityQueue<Part2MazeWalker, int>();

        var mazeWalker = new Part2MazeWalker(0, startCoordinate, 0);
        queue.Enqueue(mazeWalker, mazeWalker.Steps);

        while (queue.TryDequeue(out mazeWalker, out _))
        {
            if (mazeWalker.Level >= visited.Count)
            {
                visited.Add(new BitGrid(_grid.Height, _grid.Width));
            }

            if (visited[mazeWalker.Level][mazeWalker.Coordinate])
            {
                continue;
            }

            visited[mazeWalker.Level][mazeWalker.Coordinate] = true;

            if (mazeWalker.Level == 0 && mazeWalker.Coordinate == endCoordinate)
            {
                answer = mazeWalker.Steps;
                break;
            }

            foreach (var neighbor in _grid.SideNeighbors(mazeWalker.Coordinate))
            {
                switch (neighbor.Object)
                {
                    case Tile.Passage:
                    {
                        var newMazeWalker = new Part2MazeWalker(mazeWalker.Level, neighbor.Coordinate, mazeWalker.Steps + 1);
                        queue.Enqueue(newMazeWalker, newMazeWalker.Steps);
                        break;
                    }
                    case Tile.Portal:
                    {
                        var exitPortalCoordinate = _portalCoordinates.Values
                            .First(x => x.Contains(neighbor.Coordinate))
                            .Cast<GridCoordinate?>()
                            .FirstOrDefault(x => x != neighbor.Coordinate);

                        if (exitPortalCoordinate is not null)
                        {
                            var entranceDistance = Math.Max(Math.Abs(neighbor.Coordinate.Row - gridCenter.Row), Math.Abs(neighbor.Coordinate.Column - gridCenter.Column));
                            var exitDistance = Math.Max(Math.Abs(exitPortalCoordinate.Value.Row - gridCenter.Row), Math.Abs(exitPortalCoordinate.Value.Column - gridCenter.Column));

                            if (entranceDistance < exitDistance) // Entrance is closer to center
                            {
                                if (mazeWalker.Level < _portalCoordinates.Count) // Little speed up, do not go deeper than the number of portals
                                {
                                    var exitCoordinate = _grid
                                        .SideNeighbors(exitPortalCoordinate.Value)
                                        .First(cell => cell.Object == Tile.Passage)
                                        .Coordinate;

                                    var newMazeWalker = new Part2MazeWalker(mazeWalker.Level + 1, exitCoordinate, mazeWalker.Steps + 1);
                                    queue.Enqueue(newMazeWalker, newMazeWalker.Steps);
                                }
                            }
                            else if (mazeWalker.Level > 0)
                            {
                                var exitCoordinate = _grid
                                    .SideNeighbors(exitPortalCoordinate.Value)
                                    .First(cell => cell.Object == Tile.Passage)
                                    .Coordinate;

                                var newMazeWalker = new Part2MazeWalker(mazeWalker.Level - 1, exitCoordinate, mazeWalker.Steps + 1);
                                queue.Enqueue(newMazeWalker, newMazeWalker.Steps);
                            }                       
                        }

                        break;
                    }
                }
            }
        }

        return new PuzzleAnswer(answer, 4986);
    }
}