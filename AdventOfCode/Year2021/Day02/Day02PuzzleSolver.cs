using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day02;

[Puzzle(2021, 2, "Dive!")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private List<Command> _commands = [];

    public void ParseInput(string[] inputLines)
    {
        _commands = inputLines.Select(Command.Parse)
                              .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var submarine = new Submarine
        {
            HorizontalPosition = 0,
            Depth = 0
        };

        foreach (var command in _commands)
        {
            switch (command.Direction)
            {
                case Direction.Forward:
                    submarine.HorizontalPosition += command.Units;
                    break;
                case Direction.Up:
                    submarine.Depth -= command.Units;
                    break;
                case Direction.Down:
                    submarine.Depth += command.Units;
                    break;
            }
        }

        var answer = submarine.HorizontalPosition * submarine.Depth;

        return new PuzzleAnswer(answer, 2027977L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var aim = 0;

        var submarine = new Submarine
        {
            HorizontalPosition = 0,
            Depth = 0
        };

        foreach (var command in _commands)
        {
            switch (command.Direction)
            {
                case Direction.Forward:
                    submarine.HorizontalPosition += command.Units;
                    submarine.Depth += aim * command.Units;
                    break;
                case Direction.Up:
                    aim -= command.Units;
                    break;
                case Direction.Down:
                    aim += command.Units;
                    break;
            }
        }

        var answer = submarine.HorizontalPosition * submarine.Depth;

        return new PuzzleAnswer(answer, 1903644897L);
    }
}