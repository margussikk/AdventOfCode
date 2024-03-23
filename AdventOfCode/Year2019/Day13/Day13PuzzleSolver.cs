using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day13;

[Puzzle(2019, 13, "Care Package")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SelectToLongs(',');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computer = new IntCodeComputer(_program);

        var result = computer.Run();

        var answer = 0L;

        foreach(var chunk in result.Outputs.Chunk(3))
        {
            // ignore [0] X, Column
            // ignore [1] Y, Row

            var tile = (Tile)chunk[2];
            if (tile == Tile.Block)
            {
                answer++;
            }
        }

        return new PuzzleAnswer(answer, 253);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var scoreCoordinate = new GridCoordinate(0, -1);

        var blockCoordinates = new HashSet<GridCoordinate>();

        var computer = new IntCodeComputer(_program);
        computer.Memory[0] = 2;

        var answer = 0L;

        long joystickTilt = -2;

        while (true)
        {
            var ballCoordinate = new GridCoordinate(0, 0);
            var paddleCoordinate = new GridCoordinate(0, 0);

            IntCodeResult result;

            if (joystickTilt == -2)
            {
                result = computer.Run();
            }
            else
            {
                result = computer.Run(joystickTilt);
            }
            
            foreach(var chunk in result.Outputs.Chunk(3))
            {
                var column = (int)chunk[0];
                var row = (int)chunk[1];
                var outputValue = chunk[2];

                var coordinate = new GridCoordinate(row, column);
                if (coordinate == scoreCoordinate)
                {
                    answer = outputValue;
                }
                else
                {
                    switch ((Tile)outputValue)
                    {
                        case Tile.Empty:
                            blockCoordinates.Remove(coordinate);
                            break;
                        case Tile.Block:
                            blockCoordinates.Add(coordinate);
                            break;
                        case Tile.HorizontalPaddle:
                            paddleCoordinate = coordinate;
                            break;
                        case Tile.Ball:
                            ballCoordinate = coordinate;
                            break;                        
                    }                    
                }
            }

            if (result.ExitCode == IntCodeExitCode.Halted)
            {
                break;
            }

            joystickTilt = ballCoordinate.Column.CompareTo(paddleCoordinate.Column);
        }

        return new PuzzleAnswer(answer, 12263);
    }

    private void PrintScreen()
    {
        var screen = new Dictionary<GridCoordinate, Tile>();

        var computer = new IntCodeComputer(_program);

        var result = computer.Run();

        foreach(var chunk in result.Outputs.Chunk(3))
        {
            var column = chunk[0];
            var row = chunk[1];
            var coordinate = new GridCoordinate((int)row, (int)column);

            screen[coordinate] = (Tile)chunk[2];
        }

        var maxRow = screen.Max(kvp => kvp.Key.Row);
        var maxColumn = screen.Max(kvp => kvp.Key.Column);

        for (var row = 0; row <= maxRow; row++)
        {
            for (var column = 0; column <= maxColumn; column++)
            {
                var coordinate = new GridCoordinate(row, column);
                var tile = screen[coordinate];

                var character = tile switch
                {
                    Tile.Empty => ' ',
                    Tile.Wall => '#',
                    Tile.Block => '~',
                    Tile.HorizontalPaddle => '=',
                    Tile.Ball => 'O',
                    _ => throw new InvalidOperationException($"Invalid tile {tile}")
                };

                Console.Write(character);
            }

            Console.WriteLine();
        }
    }
}