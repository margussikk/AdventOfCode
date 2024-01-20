using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Mathematics;
using BenchmarkDotNet.Columns;

namespace AdventOfCode.Year2022.Day24;

[Puzzle(2022, 24, "Blizzard Basin")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private GridCoordinate _startLocation;

    private GridCoordinate _endLocation;

    private int _height;

    private int _width;

    private readonly List<GridCoordinate> _blizzardUpLocations = [];

    private readonly List<GridCoordinate> _blizzardDownLocations = [];

    private readonly List<GridCoordinate> _blizzardLeftLocations = [];

    private readonly List<GridCoordinate> _blizzardRightLocations = [];

    public void ParseInput(string[] inputLines)
    {
        _height = inputLines.Length;
        _width = inputLines[0].Length;

        _startLocation = new GridCoordinate(0, inputLines[0].IndexOf('.'));
        _endLocation = new GridCoordinate(inputLines.Length - 1, inputLines[^1].IndexOf('.'));

        for (var row = 1; row < inputLines.Length - 1; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                var character = inputLines[row][column];

                if (character == '^')
                {
                    _blizzardUpLocations.Add(new GridCoordinate(row, column));
                }
                else if (character == 'v')
                {
                    _blizzardDownLocations.Add(new GridCoordinate(row, column));
                }
                else if (character == '<')
                {
                    _blizzardLeftLocations.Add(new GridCoordinate(row, column));
                }
                else if (character == '>')
                {
                    _blizzardRightLocations.Add(new GridCoordinate(row, column));
                }
                else
                {
                    // Ignore walls '#' and empty spaces '.'
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(1);

        return new PuzzleAnswer(answer, 343);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(3);

        return new PuzzleAnswer(answer, 960);
    }

    private int GetAnswer(int trips)
    {
        var trip = 0;
        var startLocation = _startLocation;
        var endLocation = _endLocation;
        var queue = new PriorityQueue<GridWalker, int>();
        var visited = new HashSet<State>();
        var valleys = new List<Grid<ValleyTile>>();

        var expeditionLocation = startLocation;

        var blizzardLocations = new Dictionary<ValleyTile, List<GridCoordinate>>()
        {
            [ValleyTile.UpBlizzard] = _blizzardUpLocations,
            [ValleyTile.DownBlizzard] = _blizzardDownLocations,
            [ValleyTile.LeftBlizzard] = _blizzardLeftLocations,
            [ValleyTile.RightBlizzard] = _blizzardRightLocations,
        };

        var lcm = MathFunctions.LeastCommonMultiple(_height - 2, _width - 2);

        var valley = BuildValley(blizzardLocations);
        valleys.Add(valley);

        var walker = new GridWalker(expeditionLocation, expeditionLocation, GridDirection.None, 0);
        var distance = MeasurementFunctions.ManhattanDistance(walker.CurrentCoordinate, endLocation);
        queue.Enqueue(walker, distance);

        while (queue.TryDequeue(out walker, out _))
        {
            if (walker.CurrentCoordinate == endLocation)
            {
                trip++;
                if (trip == trips)
                {
                    return walker.Steps;
                }

                (endLocation, startLocation) = (startLocation, endLocation);

                visited.Clear();

                queue.Clear();

                distance = MeasurementFunctions.ManhattanDistance(walker.CurrentCoordinate, endLocation);
                queue.Enqueue(walker, walker.Steps + distance);

                continue;
            }

            var state = new State(walker.Steps, walker.CurrentCoordinate);
            if (visited.Contains(state))
            {
                continue;
            }

            visited.Add(state);

            // Next state
            var valleyIndex = (walker.Steps + 1) % lcm;
            if (valleys.Count <= valleyIndex)
            {
                blizzardLocations = BuildNextBlizzardLocations(blizzardLocations);
                valley = BuildValley(blizzardLocations);

                valleys.Add(valley);
            }

            valley = valleys[valleyIndex];
            var movementLocations = GetEmptyMovementLocations(valley, walker.CurrentCoordinate);
            foreach (var movementLocation in movementLocations)
            {
                if (walker.CurrentCoordinate == startLocation || movementLocation != startLocation) // Allow staying at the start, but not come back
                {
                    var nextWalker = walker.Clone();

                    var direction = walker.CurrentCoordinate.DirectionToward(movementLocation);
                    nextWalker.Move(direction);

                    distance = MeasurementFunctions.ManhattanDistance(nextWalker.CurrentCoordinate, endLocation);
                    queue.Enqueue(nextWalker, nextWalker.Steps + distance);
                }
            }
        }

        throw new InvalidOperationException();
    }

    private Grid<ValleyTile> BuildValley(Dictionary<ValleyTile, List<GridCoordinate>> blizzardLocations)
    {
        var valley = new Grid<ValleyTile>(_height, _width);

        // Horizontal walls
        for (var column = 0; column < valley.Width; column++)
        {
            valley[0, column] = ValleyTile.Wall;
            valley[valley.LastRowIndex, column] = ValleyTile.Wall;
        }

        // Vertical walls
        for (var row = 1; row < valley.LastRowIndex; row++)
        {
            valley[row, 0] = ValleyTile.Wall;
            valley[row, valley.LastColumnIndex] = ValleyTile.Wall;
        }

        // Start and end
        valley[_startLocation] = ValleyTile.Empty;
        valley[_endLocation] = ValleyTile.Empty;

        // Blizzards
        foreach (var kvp in blizzardLocations)
        {
            foreach (var location in kvp.Value)
            {
                valley[location] = valley[location] switch
                {
                    ValleyTile.Empty => kvp.Key,
                    ValleyTile.ThreeBlizzards => ValleyTile.FourBlizzards,
                    ValleyTile.TwoBlizzards => ValleyTile.ThreeBlizzards,
                    ValleyTile.UpBlizzard or
                    ValleyTile.DownBlizzard or
                    ValleyTile.LeftBlizzard or
                    ValleyTile.RightBlizzard => ValleyTile.TwoBlizzards,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        return valley;
    }

    private static void PrintValley(Grid<ValleyTile> valley, GridCoordinate expeditionLocation)
    {
        for (var row = 0; row < valley.Height; row++)
        {
            for (var column = 0; column < valley.Width; column++)
            {
                var character = valley[row, column] switch
                {
                    ValleyTile.Empty => '.',
                    ValleyTile.Wall => '#',
                    ValleyTile.UpBlizzard => '^',
                    ValleyTile.DownBlizzard => 'v',
                    ValleyTile.LeftBlizzard => '<',
                    ValleyTile.RightBlizzard => '>',
                    ValleyTile.TwoBlizzards => '2',
                    ValleyTile.ThreeBlizzards => '3',
                    ValleyTile.FourBlizzards => '4',
                    _ => ' ',
                };

                if (row == expeditionLocation.Row && column == expeditionLocation.Column)
                {
                    character = 'E';
                }

                Console.Write(character);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private Dictionary<ValleyTile, List<GridCoordinate>> BuildNextBlizzardLocations(Dictionary<ValleyTile, List<GridCoordinate>> currentBlizzardLocations)
    {
        var wallThickness = 1;
        var areaRowCount = _height - 2 * wallThickness;
        var areaColumnCount = _width - 2 * wallThickness;

        var nextBlizzardLocations = new Dictionary<ValleyTile, List<GridCoordinate>>
        {
            [ValleyTile.UpBlizzard] = currentBlizzardLocations[ValleyTile.UpBlizzard]
                .Select(x => new GridCoordinate(Move(x.Row, -1, areaRowCount), x.Column))
                .ToList(),

            [ValleyTile.DownBlizzard] = currentBlizzardLocations[ValleyTile.DownBlizzard]
                .Select(x => new GridCoordinate(Move(x.Row, 1, areaRowCount), x.Column))
                .ToList(),

            [ValleyTile.LeftBlizzard] = currentBlizzardLocations[ValleyTile.LeftBlizzard]
                .Select(x => new GridCoordinate(x.Row, Move(x.Column, -1, areaColumnCount)))
                .ToList(),

            [ValleyTile.RightBlizzard] = currentBlizzardLocations[ValleyTile.RightBlizzard]
                .Select(x => new GridCoordinate(x.Row, Move(x.Column, 1, areaColumnCount)))
                .ToList(),
        };


        return nextBlizzardLocations;

        int Move(int value, int offset, int valueCount)
        {
            return MathFunctions.Modulo(value - 1 + offset, valueCount) + wallThickness;
        }
    }

    private static IEnumerable<GridCoordinate> GetEmptyMovementLocations(Grid<ValleyTile> valley, GridCoordinate expeditionLocation)
    {
        if (valley[expeditionLocation] == ValleyTile.Empty)
        {
            yield return expeditionLocation;
        }

        foreach (var neighbor in valley.SideNeighbors(expeditionLocation))
        {
            if (neighbor.Object == ValleyTile.Empty)
            {
                yield return neighbor.Coordinate;
            }
        }
    }

    private sealed record State(int Minute, GridCoordinate ExpeditionLocation);
}