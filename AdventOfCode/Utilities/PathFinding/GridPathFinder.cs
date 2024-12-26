using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Utilities.PathFinding;
internal class GridPathFinder<T>
{
    private readonly Grid<T> _grid;
    private Func<GridCoordinatePathWalker, GridCell<T>, bool> _filterFunc = (_, _) => true;
    private Func<GridCoordinate, GridCoordinate, int, int> _costFunc = (_, _, x) => x + 1;

    public GridPathFinder(Grid<T> grid)
    {
        _grid = grid;
    }

    public GridPathFinder<T> UseFilterFunction(Func<GridCoordinatePathWalker, GridCell<T>, bool> filterFunc)
    {
        _filterFunc = filterFunc;
        return this;
    }

    public GridPathFinder<T> UseCostFunction(Func<GridCoordinate, GridCoordinate, int, int> costFunc)
    {
        _costFunc = costFunc;
        return this;
    }

    public int FindLowestCost(GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        WalkShortestPath(FindMode.FindLowestCost, startCoordinate, endCoordinate, out var lowestCost, out _);
        return lowestCost;
    }

    public List<List<GridCoordinate>> FindAllShortestPaths(GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        WalkShortestPath(FindMode.FindShortestPaths, startCoordinate, endCoordinate, out _, out var shortestPaths);
        return shortestPaths;
    }

    public Dictionary<GridCoordinate, List<int>> FindAllPathCosts(GridCoordinate startCoordinate, Func<GridCoordinatePathWalker, bool> endCondition)
    {
        WalkAllPaths(FindMode.FindAllPathCosts, startCoordinate, endCondition, out var pathCosts, out _);
        return pathCosts;
    }

    public Dictionary<GridCoordinate, List<List<GridCoordinate>>> FindAllPaths(GridCoordinate startCoordinate, Func<GridCoordinatePathWalker, bool> endCondition)
    {
        WalkAllPaths(FindMode.FindAllPaths, startCoordinate, endCondition, out _, out var paths);
        return paths;
    }

    // BFS
    public Dictionary<GridCoordinate, GridDirection> FloodFill(GridCoordinate coordinate, Func<GridCell<T>, bool> predicate)
    {
        var visitedMap = new BitGrid(_grid.Height, _grid.Width);

        var regionCoordinates = new Dictionary<GridCoordinate, GridDirection>();

        var queue = new Queue<GridCoordinate>();
        queue.Enqueue(coordinate);

        while (queue.TryDequeue(out coordinate))
        {
            if (visitedMap[coordinate])
            {
                continue;
            }

            visitedMap[coordinate] = true;

            var neighborCells = _grid.SideNeighbors(coordinate)
                                     .Where(predicate)
                                     .ToList();

            var edges = GridDirection.AllSides;

            foreach (var neighborCell in neighborCells)
            {
                edges = edges.Clear(coordinate.DirectionToward(neighborCell.Coordinate));
                queue.Enqueue(neighborCell.Coordinate);
            }

            regionCoordinates[coordinate] = edges;
        }

        return regionCoordinates;
    }

    // Dijkstra
    private void WalkShortestPath(FindMode mode, GridCoordinate startCoordinate, GridCoordinate endCoordinate, out int lowestCost, out List<List<GridCoordinate>> shortestPaths)
    {
        lowestCost = int.MaxValue;
        shortestPaths = [];

        var coordinateLowestCosts = new Dictionary<GridCoordinate, int>();
        var coordinatePreviousCoordinates = new Dictionary<GridCoordinate, HashSet<GridCoordinate>>();

        var walkerQueue = new PriorityQueue<GridCoordinatePathWalker, int>();
        var walker = new GridCoordinatePathWalker
        {
            Coordinate = startCoordinate,
            Cost = 0
        };
        walkerQueue.Enqueue(walker, 0);

        while (walkerQueue.TryDequeue(out walker, out _))
        {
            if (walker.Coordinate == endCoordinate)
            {
                if (mode == FindMode.FindLowestCost)
                {
                    return;
                }
                else if (mode == FindMode.FindShortestPaths)
                {

                    if (walker.Cost < lowestCost)
                    {
                        lowestCost = walker.Cost;

                        coordinateLowestCosts[walker.Coordinate] = walker.Cost;
                        coordinatePreviousCoordinates[walker.Coordinate] = [];
                    }

                    if (walker.PreviousCoordinate.HasValue)
                    {
                        coordinatePreviousCoordinates[walker.Coordinate].Add(walker.PreviousCoordinate.Value);
                    }

                    continue;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            if (walker.Cost > lowestCost)
            {
                continue;
            }

            var currentLowestCost = coordinateLowestCosts.GetValueOrDefault(walker.Coordinate, int.MaxValue);

            if (walker.Cost > currentLowestCost)
            {
                continue;
            }

            if (walker.Cost < currentLowestCost)
            {
                coordinateLowestCosts[walker.Coordinate] = walker.Cost;

                if (mode == FindMode.FindShortestPaths)
                {
                    coordinatePreviousCoordinates[walker.Coordinate] = [];
                }
            }

            if (mode == FindMode.FindShortestPaths && walker.PreviousCoordinate.HasValue)
            {
                coordinatePreviousCoordinates[walker.Coordinate].Add(walker.PreviousCoordinate.Value);
            }

            foreach (var neighborCell in _grid.SideNeighbors(walker.Coordinate).Where(cell => _filterFunc(walker, cell)))
            {
                currentLowestCost = coordinateLowestCosts.GetValueOrDefault(neighborCell.Coordinate, int.MaxValue);

                var newCost = _costFunc(walker.Coordinate, neighborCell.Coordinate, walker.Cost);
                if (newCost > currentLowestCost) continue;

                var newWalker = new GridCoordinatePathWalker
                {
                    Coordinate = neighborCell.Coordinate,
                    PreviousCoordinate = walker.Coordinate,
                    Cost = newCost,
                };

                var distance = MeasurementFunctions.ManhattanDistance(walker.Coordinate, newWalker.Coordinate);
                walkerQueue.Enqueue(newWalker, distance);
            }
        }

        if (mode == FindMode.FindShortestPaths && lowestCost != int.MaxValue)
        {
            shortestPaths = BuildPaths(coordinatePreviousCoordinates, startCoordinate, endCoordinate);
        }
    }

    // BFS
    private void WalkAllPaths(FindMode mode, GridCoordinate startCoordinate, Func<GridCoordinatePathWalker, bool> endCondition, out Dictionary<GridCoordinate, List<int>> pathCosts, out Dictionary<GridCoordinate, List<List<GridCoordinate>>> paths)
    {
        pathCosts = [];
        paths = [];

        var walker = new GridCoordinatePathWalker()
        {
            Coordinate = startCoordinate,
        };

        var walkerQueue = new Queue<GridCoordinatePathWalker>();
        walkerQueue.Enqueue(walker);

        while (walkerQueue.TryDequeue(out walker))
        {
            if (mode == FindMode.FindAllPaths)
            {
                walker.Path.Add(walker.Coordinate);
            }

            if (endCondition(walker))
            {
                if (mode == FindMode.FindAllPathCosts)
                {
                    pathCosts.AddToValueList(walker.Coordinate, walker.Cost);
                }
                else if (mode == FindMode.FindAllPaths)
                {
                    paths.AddToValueList(walker.Coordinate, walker.Path);
                }

                continue;
            }

            foreach (var neighborCell in _grid.SideNeighbors(walker.Coordinate)
                                              .Where(c => _filterFunc(walker, c)))
            {
                var newWalker = new GridCoordinatePathWalker
                {
                    Coordinate = neighborCell.Coordinate,
                    Cost = _costFunc(walker.Coordinate, neighborCell.Coordinate, walker.Cost)
                };

                if (mode == FindMode.FindAllPaths)
                {
                    newWalker.Path.AddRange(walker.Path);
                }

                walkerQueue.Enqueue(newWalker);
            }
        }
    }


    private enum FindMode
    {
        FindLowestCost,
        FindShortestPaths,
        FindAllPathCosts,
        FindAllPaths
    }

    private static List<List<GridCoordinate>> BuildPaths(Dictionary<GridCoordinate, HashSet<GridCoordinate>> coordinatePreviousCoordinates, GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        if (startCoordinate == endCoordinate)
        {
            return
            [
                [startCoordinate],
            ];
        }

        var mainList = new List<List<GridCoordinate>>();

        var queue = new Queue<List<GridCoordinate>>();
        queue.Enqueue([endCoordinate]);

        while (queue.Count > 0)
        {
            var newQueue = new Queue<List<GridCoordinate>>();

            while (queue.TryDequeue(out var path))
            {
                if (path[0] == startCoordinate)
                {
                    mainList.Add(path);
                    continue;
                }

                foreach (var previousCoordinate in coordinatePreviousCoordinates[path[0]])
                {
                    newQueue.Enqueue([previousCoordinate, .. path]);
                }
            }

            queue = newQueue;
        }

        return mainList;
    }
}
