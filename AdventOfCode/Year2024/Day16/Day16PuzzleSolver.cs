using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day16;

[Puzzle(2024, 16, "Reindeer Maze")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private BitGrid _maze = new(0, 0);

    private GridCoordinate _startCoordinate = new(0, 0);
    private GridCoordinate _endCoordinate = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _maze = new BitGrid(inputLines.Length, inputLines[0].Length);

        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                var character = inputLines[row][column];

                if (character == 'S')
                {
                    _startCoordinate = new GridCoordinate(row, column);
                }
                else if (character == 'E')
                {
                    _endCoordinate = new GridCoordinate(row, column);
                }

                _maze[row, column] = character == '#';
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = FindLowestScore();

        return new PuzzleAnswer(answer, 102488);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = FindBestPathCoordinatesCount();

        return new PuzzleAnswer(answer, 559);
    }

    // Dijkstra
    private int FindLowestScore()
    {
        var lowestScoreGrid = new Grid<int?>(_maze.Height, _maze.Width);

        var walkerQueue = new PriorityQueue<MazeWalker, int>();
        var walker = new MazeWalker
        {
            Position = new GridPosition(_startCoordinate, GridDirection.Right),
            Score = 0
        };

        walkerQueue.Enqueue(walker, walker.Score);

        while (walkerQueue.TryDequeue(out walker, out _))
        {
            if (walker.Position.Coordinate == _endCoordinate)
            {
                return walker.Score;
            }

            if (lowestScoreGrid[walker.Position.Coordinate].HasValue)
            {
                continue;
            }

            lowestScoreGrid[walker.Position.Coordinate] = walker.Score;

            foreach (var neighborCell in _maze.SideNeighbors(walker.Position.Coordinate).Where(c => !c.Object))
            {
                var currentPoints = lowestScoreGrid[neighborCell.Coordinate] ?? int.MaxValue;

                var directionToward = walker.Position.Coordinate.DirectionToward(neighborCell.Coordinate);

                var newPoints = walker.Score + 1;

                if (directionToward != walker.Position.Direction)
                {
                    newPoints += 1000;
                }

                if (newPoints >= currentPoints) continue;

                var newWalker = new MazeWalker
                {
                    Position = new GridPosition(neighborCell.Coordinate, directionToward),
                    Score = newPoints
                };

                walkerQueue.Enqueue(newWalker, newWalker.Score);
            }
        }

        return 0;
    }

    private int FindBestPathCoordinatesCount()
    {
        var lowestScore = int.MaxValue;

        var positionLowestScore = new Dictionary<GridPosition, int>();
        var positionPreviousPositions = new Dictionary<GridPosition, HashSet<GridPosition?>>();
        var endPositions = new HashSet<GridPosition>();

        var walkerQueue = new PriorityQueue<MazeWalker, int>();
        var walker = new MazeWalker
        {
            Position = new GridPosition(_startCoordinate, GridDirection.Right),
            Score = 0,
        };

        walkerQueue.Enqueue(walker, walker.Score);

        while (walkerQueue.TryDequeue(out walker, out _))
        {
            if (walker.Position.Coordinate == _endCoordinate)
            {
                if (walker.Score < lowestScore)
                {
                    lowestScore = walker.Score;
                    endPositions = [];

                    positionLowestScore[walker.Position] = walker.Score;
                    positionPreviousPositions[walker.Position] = [];
                }

                endPositions.Add(walker.Position);
                positionPreviousPositions[walker.Position].Add(walker.PreviousPosition);

                continue;
            }

            if (walker.Score > lowestScore)
            {
                continue;
            }

            var currentLowestScore = positionLowestScore.GetValueOrDefault(walker.Position, int.MaxValue);

            if (walker.Score > currentLowestScore)
            {
                continue;
            }

            if (walker.Score < currentLowestScore)
            {
                positionLowestScore[walker.Position] = walker.Score;
                positionPreviousPositions[walker.Position] = [];
            }

            positionPreviousPositions[walker.Position].Add(walker.PreviousPosition);

            foreach (var nextPosition in walker.GetNextPositions())
            {
                if (_maze[nextPosition.Coordinate]) continue; // A wall

                var newScore = walker.Score;

                if (nextPosition.Direction != walker.Position.Direction)
                {
                    newScore += 1000;
                }

                if (nextPosition.Coordinate != walker.Position.Coordinate)
                {
                    newScore++;
                }

                currentLowestScore = positionLowestScore.GetValueOrDefault(nextPosition, int.MaxValue);
                if (newScore >= currentLowestScore) continue;

                var newWalker = new MazeWalker
                {
                    Position = nextPosition,
                    PreviousPosition = walker.Position,
                    Score = newScore,
                };

                walkerQueue.Enqueue(newWalker, newWalker.Score);
            }
        }

        var bestPathCoordinates = new HashSet<GridCoordinate>();

        var positions = endPositions;
        while (positions.Count != 0)
        {
            var previousPositions = new HashSet<GridPosition>();
            foreach (var position in positions)
            {
                bestPathCoordinates.Add(position.Coordinate);
                previousPositions.UnionWith(positionPreviousPositions[position].Where(x => x != null)
                                                                               .Select(x => x!.Value));
            }

            positions = previousPositions;
        }

        return bestPathCoordinates.Count;
    }
}