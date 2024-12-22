using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Utilities.PathFinding;
internal class GridPathFinder<T>
{
    private Func<GridCell<T>, bool> _filterFunc = (x) => true;
    private Func<GridCoordinate, GridCoordinate, int, int> _costFunc = (_, _, x) => x + 1;

    public GridPathFinder<T> UseFilterFunction(Func<GridCell<T>, bool> filterFunc)
    {
        _filterFunc = filterFunc;
        return this;
    }

    public GridPathFinder<T> UseCostFunction(Func<GridCoordinate, GridCoordinate, int, int> costFunc)
    {
        _costFunc = costFunc;
        return this;
    }

    public int FindLowestCost(Grid<T> grid, GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        FindShortestPath(FindMode.FindLowestCost, grid, startCoordinate, endCoordinate, out var lowestCost, out _);
        return lowestCost;
    }

    public List<List<GridCoordinate>> FindAllShortestPaths(Grid<T> grid, GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        FindShortestPath(FindMode.FindShortestPaths, grid, startCoordinate, endCoordinate, out _, out var shortestPaths);
        return shortestPaths;
    }

    private void FindShortestPath(FindMode mode, Grid<T> grid, GridCoordinate startCoordinate, GridCoordinate endCoordinate, out int lowestCost, out List<List<GridCoordinate>> shortestPaths)
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

            foreach (var neigbor in grid.SideNeighbors(walker.Coordinate).Where(_filterFunc))
            {
                currentLowestCost = coordinateLowestCosts.GetValueOrDefault(neigbor.Coordinate, int.MaxValue);

                var newCost = _costFunc(walker.Coordinate, neigbor.Coordinate, walker.Cost);
                if (newCost > currentLowestCost) continue;

                var newWalker = new GridCoordinatePathWalker
                {
                    Coordinate = neigbor.Coordinate,
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


    private enum FindMode
    {
        FindLowestCost,
        FindShortestPaths,
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
