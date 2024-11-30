using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day23;

[Puzzle(2019, 23, "Category Six")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SelectToLongs(',');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computers = new IntCodeComputer[50];
        var computerInputs = new List<long>[50];

        // Init
        for (var computerIndex = 0; computerIndex < computers.Length; computerIndex++)
        {
            var computer = new IntCodeComputer(_program);

            computers[computerIndex] = computer;
            computerInputs[computerIndex] = [computerIndex];
        }

        // Exchange packets
        while (true)
        {
            for (var computerIndex = 0; computerIndex < computers.Length; computerIndex++)
            {
                var computer = computers[computerIndex];
                var inputs = computerInputs[computerIndex];

                if (inputs.Count == 0)
                {
                    inputs.Add(-1);
                }

                var result = computer.Run(inputs);
                inputs.Clear();

                foreach (var chunk in result.Outputs.Chunk(3))
                {
                    var destination = chunk[0];
                    var x = chunk[1];
                    var y = chunk[2];

                    if (destination == 255)
                    {
                        return new PuzzleAnswer(y, 22659);
                    }

                    computerInputs[destination].Add(x);
                    computerInputs[destination].Add(y);
                }
            }
        }
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computers = new IntCodeComputer[50];
        var computerInputs = new List<long>[50];

        // Init
        for (var computerIndex = 0; computerIndex < computers.Length; computerIndex++)
        {
            var computer = new IntCodeComputer(_program);

            computers[computerIndex] = computer;
            computerInputs[computerIndex] = [computerIndex];
        }

        long answer;

        var natX = -1L;
        var natY = -1L;

        var lastNatY = long.MinValue;

        // Exchange packets
        while (true)
        {
            var idle = true;

            for (var computerIndex = 0; computerIndex < computers.Length; computerIndex++)
            {
                var computer = computers[computerIndex];
                var inputs = computerInputs[computerIndex];

                if (inputs.Count == 0)
                {
                    inputs.Add(-1);
                }

                var result = computer.Run(inputs);
                inputs.Clear();

                foreach (var chunk in result.Outputs.Chunk(3))
                {
                    idle = false;

                    var destination = chunk[0];
                    var x = chunk[1];
                    var y = chunk[2];

                    if (destination == 255)
                    {
                        natX = x;
                        natY = y;
                    }
                    else
                    {
                        computerInputs[destination].Add(x);
                        computerInputs[destination].Add(y);
                    }
                }
            }

            if (!idle) continue;

            if (lastNatY == natY)
            {
                answer = natY;
                break;
            }

            computerInputs[0].Add(natX);
            computerInputs[0].Add(natY);

            lastNatY = natY;
        }


        return new PuzzleAnswer(answer, 17429);
    }
}