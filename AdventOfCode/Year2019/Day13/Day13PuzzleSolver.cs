using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day13;

[Puzzle(2019, 13, "Care Package")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();

    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computer = new IntCodeComputer();
        computer.Load(_program);
        computer.Run();

        var answer = 0L;

        while(computer.Outputs.Count > 0)
        {
            computer.Outputs.Dequeue(); // X, Column
            computer.Outputs.Dequeue(); // Y, Row

            var tile = (Tile)computer.Outputs.Dequeue();
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

        var computer = new IntCodeComputer();
        computer.Load(_program);
        computer.WriteMemory(0, 2);

        var answer = 0L;
        
        while(true)
        {
            var ballCoordinate = new GridCoordinate(0, 0);
            var paddleCoordinate = new GridCoordinate(0, 0);

            var exitCode = computer.Run();
            
            while (computer.Outputs.Count > 0)
            {
                var column = computer.Outputs.Dequeue();
                var row = computer.Outputs.Dequeue();
                var outputValue = computer.Outputs.Dequeue();

                var coordinate = new GridCoordinate((int)row, (int)column);
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

            if (exitCode == IntCodeExitCode.Halted)
            {
                break;
            }

            var joystickTilt = ballCoordinate.Column.CompareTo(paddleCoordinate.Column);
            computer.Inputs.Enqueue(joystickTilt);
        }

        return new PuzzleAnswer(answer, 12263);
    }

    private void PrintScreen()
    {
        var screen = new Dictionary<GridCoordinate, Tile>();

        var computer = new IntCodeComputer();
        computer.Load(_program);
        computer.Run();

        while (computer.Outputs.Count > 0)
        {
            var column = computer.Outputs.Dequeue();
            var row = computer.Outputs.Dequeue();
            var coordinate = new GridCoordinate((int)row, (int)column);

            var tile = (Tile)computer.Outputs.Dequeue();

            screen[coordinate] = tile;
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