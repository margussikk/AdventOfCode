using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day15;

[Puzzle(2024, 15, "Warehouse Woes")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _warehouseMap = new(0, 0);
    private GridCoordinate _robotStartCoordinate;
    private List<GridDirection> _moveDirections = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        // Warehouse map
        _warehouseMap = chunks[0].SelectToGrid((character, coordinate) =>
        {
            if (character == '@')
            {
                _robotStartCoordinate = coordinate;
            }

            return character switch
            {
                'O' => Tile.Box,
                '#' => Tile.Wall,
                _ => Tile.Empty,
            };
        });

        // Moves
        _moveDirections = chunks[1]
            .SelectMany(line => line.Select(character => character.ParseToGridDirection()))
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var robotCoordinate = _robotStartCoordinate;
        var warehouseMap = _warehouseMap.Clone();

        foreach (var moveDirection in _moveDirections)
        {
            var nextRobotCoordinate = robotCoordinate.Move(moveDirection);

            if (warehouseMap[nextRobotCoordinate] == Tile.Empty)
            {
                robotCoordinate = nextRobotCoordinate;
            }
            else if (warehouseMap[nextRobotCoordinate] == Tile.Box)
            {
                var boxCoordinate = nextRobotCoordinate;

                while (warehouseMap[boxCoordinate] == Tile.Box)
                {
                    boxCoordinate = boxCoordinate.Move(moveDirection);
                }

                if (warehouseMap[boxCoordinate] == Tile.Empty)
                {
                    warehouseMap[boxCoordinate] = Tile.Box;
                    warehouseMap[nextRobotCoordinate] = Tile.Empty;
                    robotCoordinate = nextRobotCoordinate;
                }
            }
        }

        var answer = warehouseMap.Where(c => c.Object == Tile.Box)
                                 .Sum(c => c.Coordinate.Row * 100 + c.Coordinate.Column);

        return new PuzzleAnswer(answer, 1430536);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var robotCoordinate = new GridCoordinate(_robotStartCoordinate.Row, _robotStartCoordinate.Column * 2);
        var warehouseMap = new Grid<Tile>(_warehouseMap.Height, _warehouseMap.Width * 2);

        foreach (var cell in _warehouseMap)
        {
            var coordinate = new GridCoordinate(cell.Coordinate.Row, cell.Coordinate.Column * 2);

            if (cell.Object == Tile.Box)
            {
                warehouseMap[coordinate] = Tile.BoxLeft;
                warehouseMap[coordinate.Right()] = Tile.BoxRight;
            }
            else
            {
                warehouseMap[coordinate] = cell.Object;
                warehouseMap[coordinate.Right()] = cell.Object;
            }
        }

        foreach (var moveDirection in _moveDirections)
        {
            var nextRobotCoordinate = robotCoordinate.Move(moveDirection);

            if (warehouseMap[nextRobotCoordinate] == Tile.Empty)
            {
                robotCoordinate = nextRobotCoordinate;
            }
            else if (warehouseMap[nextRobotCoordinate] is Tile.BoxLeft or Tile.BoxRight)
            {
                if (moveDirection is GridDirection.Left or GridDirection.Right)
                {
                    var boxCoordinate = nextRobotCoordinate;

                    while (warehouseMap[boxCoordinate] is Tile.BoxLeft or Tile.BoxRight)
                    {
                        boxCoordinate = boxCoordinate.Move(moveDirection);
                    }

                    if (warehouseMap[boxCoordinate] == Tile.Empty)
                    {
                        var oppositeMoveDirection = moveDirection.Flip();

                        while (boxCoordinate != nextRobotCoordinate)
                        {
                            var previousBoxCoordinate = boxCoordinate.Move(oppositeMoveDirection);
                            warehouseMap[boxCoordinate] = warehouseMap[previousBoxCoordinate];
                            boxCoordinate = previousBoxCoordinate;
                        }

                        warehouseMap[nextRobotCoordinate] = Tile.Empty;
                        robotCoordinate = nextRobotCoordinate;
                    }
                }
                else // Up or down
                {
                    var couldMove = true;
                    var processedBoxLeftCoordinates = new HashSet<GridCoordinate>();

                    var boxCoordinates = GetBoxCoordinates(nextRobotCoordinate);

                    var stack = new Stack<GridCoordinate[]>();
                    stack.Push(boxCoordinates);

                    while (stack.TryPop(out boxCoordinates))
                    {
                        if (!processedBoxLeftCoordinates.Add(boxCoordinates[0]))
                        {
                            continue;
                        }

                        var nextBoxCoordinates = new GridCoordinate[]
                        {
                            boxCoordinates[0].Move(moveDirection), // Box left
                            boxCoordinates[1].Move(moveDirection), // Box right
                        };

                        foreach (var nextBoxCoordinate in nextBoxCoordinates)
                        {
                            if (warehouseMap[nextBoxCoordinate] == Tile.Wall)
                            {
                                couldMove = false;
                                break;
                            }

                            if (warehouseMap[nextBoxCoordinate] is Tile.BoxLeft or Tile.BoxRight)
                            {
                                stack.Push(GetBoxCoordinates(nextBoxCoordinate));
                            }
                        }

                        if (!couldMove)
                        {
                            break;
                        }
                    }

                    if (couldMove)
                    {
                        var orderedBoxLeftCoordinates = moveDirection == GridDirection.Up
                            ? processedBoxLeftCoordinates.OrderBy(x => x.Row)
                            : processedBoxLeftCoordinates.OrderByDescending(x => x.Row);

                        foreach (var coordinate in orderedBoxLeftCoordinates)
                        {
                            warehouseMap[coordinate] = Tile.Empty;
                            warehouseMap[coordinate.Right()] = Tile.Empty;

                            var newCoordinate = coordinate.Move(moveDirection);
                            warehouseMap[newCoordinate] = Tile.BoxLeft;
                            warehouseMap[newCoordinate.Right()] = Tile.BoxRight;
                        }

                        robotCoordinate = nextRobotCoordinate;
                    }
                }
            }
        }

        var answer = warehouseMap.Where(c => c.Object == Tile.BoxLeft)
                                 .Sum(c => c.Coordinate.Row * 100 + c.Coordinate.Column);

        return new PuzzleAnswer(answer, 1452348);

        GridCoordinate[] GetBoxCoordinates(GridCoordinate coordinate)
        {
            return warehouseMap[coordinate] == Tile.BoxLeft
                ? [coordinate, coordinate.Right()]
                : [coordinate.Left(), coordinate];
        }
    }
}