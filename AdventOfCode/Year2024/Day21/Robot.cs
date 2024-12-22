using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2024.Day21;
internal abstract class Robot<T>
{
    private readonly Grid<T> _keypad;
    private readonly T _initialButton;

    protected Robot(Grid<T> keypad, T initialButton)
    {
        _keypad = keypad;
        _initialButton = initialButton;
    }

    protected List<T> GenericPushButtons(IEnumerable<GridDirection> directions)
    {
        var coordinate = _keypad.First(cell => cell.Object is not null && cell.Object.Equals(_initialButton)).Coordinate;

        var pushedButtons = new List<T>();

        foreach (var direction in directions)
        {
            if (direction == GridDirection.Start)
            {
                pushedButtons.Add(_keypad[coordinate]);
            }
            else
            {
                coordinate = coordinate.Move(direction);
            }
        }

        return pushedButtons;
    }

    protected List<List<GridDirection>> GenericGetDirections(Dictionary<(T, T), List<List<GridDirection>>> cache,  List<T> pushedButtons, T startButton)
    {
        if (pushedButtons.Count == 0)
        {
            return
            [
                []
            ];
        }

        
        if (!cache.TryGetValue((startButton, pushedButtons[0]), out var directionsList))
        {
            var startCoordinate = _keypad.First(cell => cell.Object is not null && cell.Object.Equals(startButton)).Coordinate;
            var endCoordinate = _keypad.First(cell => cell.Object is not null && cell.Object.Equals(pushedButtons[0])).Coordinate;

            var gridPathFinder = new GridPathFinder<T>()
                .UseFilterFunction(x => x.Object is not null);

            var pathList = gridPathFinder.FindAllShortestPaths(_keypad, startCoordinate, endCoordinate);

            directionsList = [];
            foreach (var path in pathList)
            {
                var directions = new List<GridDirection>();

                for (var i = 0; i < path.Count - 1; i++)
                {
                    var direction = path[i].DirectionToward(path[i + 1]);
                    directions.Add(direction);
                }

                directions.Add(GridDirection.Start);

                directionsList.Add(directions);
            }

            var directionChangeCounts = directionsList.ToLookup(CountDirectionChanges);

            var minDirectionChangeCount = directionChangeCounts.Min(x => x.Key);

            directionsList = [.. directionChangeCounts[minDirectionChangeCount]];

            cache[(startButton, pushedButtons[0])] = directionsList;
        }

        var listOfDirectionLists = new List<List<GridDirection>>();
        foreach (var directions in directionsList)
        { 
            var childLists = GenericGetDirections(cache, pushedButtons[1..], pushedButtons[0]);
            foreach (var childList in childLists)
            {
                var newList = new List<GridDirection>();
                newList.AddRange(directions);
                newList.AddRange(childList);

                listOfDirectionLists.Add(newList);
            }
        }

        return listOfDirectionLists;
    }

    private static int CountDirectionChanges(List<GridDirection> directions)
    {
        var count = 0;

        for (var i = 1; i < directions.Count; i++)
        {
            if (directions[i - 1] != directions[i])
            {
                count++;
            }
        }

        return count;
    }
}
