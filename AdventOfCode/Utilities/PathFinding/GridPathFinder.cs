using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Utilities.PathFinding;
internal class GridPathFinder<T>
{
    private readonly IGrid<T> _grid;

    private Func<GridPathWalker, GridCell<T>, bool> _cellFilter = DefaultCellFilter;
    private Func<GridPathWalker, GridPosition, int> _costCalculator = DefaultCostCalculator;

    public GridPathFinder(IGrid<T> grid)
    {
        _grid = grid;
    }

    public GridPathFinder<T> SetCellFilter(Func<GridPathWalker, GridCell<T>, bool> cellFilter)
    {
        _cellFilter = cellFilter;
        return this;
    }

    public GridPathFinder<T> SetCostCalculator(Func<GridPathWalker, GridPosition, int> costCalculator)
    {
        _costCalculator = costCalculator;
        return this;
    }

    public int FindShortestPathLength(GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        var startPosition = new GridPosition(startCoordinate, GridDirection.None);
        return FindShortestPathLength(startPosition, endCoordinate);
    }

    public int FindShortestPathLength(GridPosition startPosition, GridCoordinate endCoordinate)
    {
        var lowestCostGrid = new Grid<int?>(_grid.Height, _grid.Width);

        var walkerQueue = new PriorityQueue<GridPathWalker, int>();
        var walker = new GridPathWalker
        {
            Position = startPosition
        };

        walkerQueue.Enqueue(walker, walker.Cost);
        while (walkerQueue.TryDequeue(out walker, out _))
        {
            if (walker.Position.Coordinate == endCoordinate)
            {
                return walker.Cost;
            }

            if (lowestCostGrid[walker.Position.Coordinate].HasValue)
            {
                continue;
            }

            lowestCostGrid[walker.Position.Coordinate] = walker.Cost;
            
            foreach (var nextPosition in walker.MovementPositions().Where(p => _grid.InBounds(p.Coordinate) && _cellFilter(walker, _grid.Cell(p.Coordinate))))
            {
                var nextCost = _costCalculator(walker, nextPosition);

                var currentLowestCost = lowestCostGrid[nextPosition.Coordinate] ?? int.MaxValue;
                if (nextCost >= currentLowestCost) continue;

                var newWalker = new GridPathWalker
                {
                    Position = nextPosition,
                    Cost = nextCost
                };

                walkerQueue.Enqueue(newWalker, newWalker.Cost);
            }
        }

        return int.MaxValue;
    }

    public List<List<GridCoordinate>> FindAllShortestPaths(GridCoordinate startCoordinate, GridCoordinate endCoordinate)
    {
        var startPosition = new GridPosition(startCoordinate, GridDirection.None);
        return FindAllShortestPaths(startPosition, endCoordinate);
    }

    public List<List<GridCoordinate>> FindAllShortestPaths(GridPosition startPosition, GridCoordinate endCoordinate)
    {
        var lowestCost = int.MaxValue;

        var lowestCosts = new Dictionary<GridPosition, int>();
        var previousPositions = new Dictionary<GridPosition, HashSet<GridPosition>>();
        var endPositions = new HashSet<GridPosition>();

        var walkerQueue = new PriorityQueue<GridPathWalker, int>();
        var walker = new GridPathWalker
        {
            Position = startPosition,
            Cost = 0
        };
        walkerQueue.Enqueue(walker, walker.Cost);

        while (walkerQueue.TryDequeue(out walker, out _))
        {
            if (walker.Position.Coordinate == endCoordinate)
            {
                if (walker.Cost < lowestCost)
                {
                    lowestCost = walker.Cost;
                    endPositions = [];

                    lowestCosts[walker.Position] = walker.Cost;
                    previousPositions[walker.Position] = [];
                }

                endPositions.Add(walker.Position);

                if (walker.PreviousPosition.HasValue)
                {
                    previousPositions[walker.Position].Add(walker.PreviousPosition.Value);
                }

                continue;
            }

            if (walker.Cost > lowestCost)
            {
                continue;
            }

            var currentLowestCost = lowestCosts.GetValueOrDefault(walker.Position, int.MaxValue);
            if (walker.Cost > currentLowestCost)
            {
                continue;
            }

            if (walker.Cost < currentLowestCost)
            {
                lowestCosts[walker.Position] = walker.Cost;
                previousPositions[walker.Position] = [];
            }

            if (walker.PreviousPosition.HasValue)
            {
                previousPositions[walker.Position].Add(walker.PreviousPosition.Value);
            }

            foreach (var nextPosition in walker.TurningPositions().Where(p => _grid.InBounds(p.Coordinate) && _cellFilter(walker, _grid.Cell(p.Coordinate))))
            {
                var nextCost = _costCalculator(walker, nextPosition);

                currentLowestCost = lowestCosts.GetValueOrDefault(nextPosition, int.MaxValue);
                if (nextCost > currentLowestCost) continue;

                var newWalker = new GridPathWalker
                {
                    Position = nextPosition,
                    PreviousPosition = walker.Position,
                    Cost = nextCost,
                };

                walkerQueue.Enqueue(newWalker, newWalker.Cost);
            }
        }

        if (lowestCost == int.MaxValue)
        {
            return [];
        }
        
        return BuildPaths(previousPositions, startPosition, [.. endPositions]);
    }

    public void WalkAllPaths(bool unique, GridCoordinate startCoordinate, Func<GridPathWalker, bool> continueFunc)
    {
        var visited = new InfiniteBitGrid();

        var walker = new GridPathWalker
        {
            Position = new GridPosition(startCoordinate, GridDirection.None)
        };

        var queue = new Queue<GridPathWalker>();
        queue.Enqueue(walker);

        while (queue.TryDequeue(out walker))
        {
            if (unique)
            {
                if (visited[walker.Position.Coordinate])
                {
                    continue;
                }

                visited[walker.Position.Coordinate] = true;
            }

            if (!continueFunc(walker))
            {
                continue;
            }

            foreach (var nextPosition in walker.MovementPositions().Where(p => _grid.InBounds(p.Coordinate) && _cellFilter(walker, _grid.Cell(p.Coordinate))))
            {
                var newWalker = new GridPathWalker
                {
                    Position = nextPosition,
                    Cost = _costCalculator(walker, nextPosition),
                };

                queue.Enqueue(newWalker);
            }
        }
    }

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

    private static List<List<GridCoordinate>> BuildPaths(Dictionary<GridPosition, HashSet<GridPosition>> previousPositions, GridPosition startPosition, List<GridPosition> endPositions)
    {
        var mainList = new List<List<GridCoordinate>>();

        var queue = new Queue<List<GridPosition>>();
        queue.Enqueue(endPositions);

        while (queue.Count > 0)
        {
            var newQueue = new Queue<List<GridPosition>>();
            while (queue.TryDequeue(out var path))
            {
                if (path[0] == startPosition)
                {
                    mainList.Add([.. path.Select(x => x.Coordinate)]);
                    continue;
                }

                foreach (var previousPosition in previousPositions[path[0]])
                {
                    newQueue.Enqueue([previousPosition, .. path]);
                }
            }

            queue = newQueue;
        }

        return mainList;
    }


    public static bool DefaultBitGridCellFilter(GridPathWalker _, GridCell<bool> cell) => !cell.Object;

    private static bool DefaultCellFilter(GridPathWalker walker, GridCell<T> cell) => true;

    private static int DefaultCostCalculator(GridPathWalker walker, GridPosition nextPosition) => walker.Cost + 1;
}
