using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2018.Day22;

[Puzzle(2018, 22, "Mode Maze")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<Region, Tool[]> _regionTools = new()
    {
        [Region.Rocky] = [Tool.ClimbingGear, Tool.Torch],
        [Region.Wet] = [Tool.ClimbingGear, Tool.Neither],
        [Region.Narrow] = [Tool.Torch, Tool.Neither],
    };

    private int _depth;
    private GridCoordinate _targetCoordinate;

    public void ParseInput(string[] inputLines)
    {
        _depth = int.Parse(inputLines[0].Split(':')[1]);
        _targetCoordinate = GridCoordinate.Parse(inputLines[1].Split(':')[1]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        var caveSystemGrid = new InfiniteGrid<Region?>();
        var erosionLevelGrid = new InfiniteGrid<int?>();

        for (var row = 0; row <= _targetCoordinate.Row; row++)
        {
            for (var column = 0; column <= _targetCoordinate.Column; column++)
            {
                var region = GetRegion(new GridCoordinate(row, column), caveSystemGrid, erosionLevelGrid);

                answer += region switch
                {
                    Region.Rocky => 0,
                    Region.Wet => 1,
                    Region.Narrow => 2,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        return new PuzzleAnswer(answer, 8575);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = int.MaxValue;

        var caveSystemGrid = new InfiniteGrid<Region?>();
        var erosionLevelGrid = new InfiniteGrid<int?>();

        var fewestMinutes = new Dictionary<(GridCoordinate, Tool), int>();

        var climber = new Climber
        {
            Coordinate = new GridCoordinate(0, 0),
            Tool = Tool.Torch,
            Minutes = 0
        };

        var climberQueue = new PriorityQueue<Climber, int>();
        climberQueue.Enqueue(climber, climber.Minutes);

        while (climberQueue.TryDequeue(out climber, out _))
        {
            if (climber.Coordinate == _targetCoordinate && climber.Tool == Tool.Torch)
            {
                answer = climber.Minutes;
                break;
            }

            var state = (climber.Coordinate, climber.Tool);

            var currentLowestMinutes = fewestMinutes.GetValueOrDefault(state, int.MaxValue);
            if (climber.Minutes >= currentLowestMinutes)
            {
                continue;
            }

            fewestMinutes[state] = climber.Minutes;

            foreach (var nextClimber in GetNextClimbers(climber, caveSystemGrid, erosionLevelGrid))
            {
                var nextState = (nextClimber.Coordinate, nextClimber.Tool);

                currentLowestMinutes = fewestMinutes.GetValueOrDefault(nextState, int.MaxValue);
                if (nextClimber.Minutes >= currentLowestMinutes) continue;

                var distance = climber.Coordinate.ManhattanDistanceTo(_targetCoordinate);
                climberQueue.Enqueue(nextClimber, nextClimber.Minutes + distance);
            }
        }

        return new PuzzleAnswer(answer, 999);
    }

    private List<Climber> GetNextClimbers(Climber climber, InfiniteGrid<Region?> caveSystemGrid, InfiniteGrid<int?> erosionLevelGrid)
    {
        var region = GetRegion(climber.Coordinate, caveSystemGrid, erosionLevelGrid);

        var climbers = new List<Climber>
        {
            // Change tool
            new() {
                Coordinate = climber.Coordinate,
                Tool = _regionTools[region].First(t => t != climber.Tool),
                Minutes = climber.Minutes + 7,
            }
        };

        // Keep the tool, but try to move
        foreach (var nextCoordinate in climber.Coordinate.SideNeighbors().Where(c => c.Row >= 0 && c.Column >= 0))
        {
            region = GetRegion(nextCoordinate, caveSystemGrid, erosionLevelGrid);
            if (_regionTools[region].Contains(climber.Tool))
            {
                climbers.Add(new Climber
                {
                    Coordinate = nextCoordinate,
                    Tool = climber.Tool,
                    Minutes = climber.Minutes + 1,
                });
            }
        }

        return climbers;
    }

    private Region GetRegion(GridCoordinate coordinate, InfiniteGrid<Region?> caveSystemGrid, InfiniteGrid<int?> erosionLevelGrid)
    {
        var region = caveSystemGrid[coordinate];
        if (region is not null)
        {
            return region.Value;
        }

        region = (GetErosionLevel(coordinate, erosionLevelGrid) % 3) switch
        {
            0 => Region.Rocky,
            1 => Region.Wet,
            2 => Region.Narrow,
            _ => throw new InvalidOperationException()
        };
        caveSystemGrid[coordinate] = region.Value;

        return region.Value;
    }

    private int GetErosionLevel(GridCoordinate coordinate, InfiniteGrid<int?> erosionLevelGrid)
    {
        var erosionLevel = erosionLevelGrid[coordinate];
        if (erosionLevel is not null)
        {
            return erosionLevel.Value;
        }

        int geologicIndex;
        if (coordinate == _targetCoordinate)
        {
            geologicIndex = 0;
        }
        else if (coordinate.Row == 0)
        {
            geologicIndex = coordinate.Column * 16807;
        }
        else if (coordinate.Column == 0)
        {
            geologicIndex = coordinate.Row * 48271;
        }
        else
        {
            geologicIndex = GetErosionLevel(coordinate.Left(), erosionLevelGrid) * GetErosionLevel(coordinate.Up(), erosionLevelGrid);
        }

        erosionLevel = (geologicIndex + _depth) % 20183;
        erosionLevelGrid[coordinate] = erosionLevel.Value;

        return erosionLevel.Value;
    }
}