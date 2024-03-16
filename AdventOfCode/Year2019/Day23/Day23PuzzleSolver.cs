using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day23;

[Puzzle(2019, 23, "Category Six")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();
    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computers = new IntCodeComputer[50];

        // Init
        for (var computerIndex = 0; computerIndex < computers.Length; computerIndex++)
        {
            var computer = new IntCodeComputer();
            computer.Load(_program);
            computer.Inputs.Enqueue(computerIndex);

            computers[computerIndex] = computer;
        }

        // Exchange packets
        while(true)
        {
            for (var computerIndex = 0; computerIndex < computers.Length; computerIndex++)
            {
                var computer = computers[computerIndex];

                if (computer.Inputs.Count == 0)
                {
                    computer.Inputs.Enqueue(-1);
                }

                computer.Run();
                while (computer.Outputs.Count != 0)
                {
                    var destination = computer.Outputs.Dequeue();
                    var x = computer.Outputs.Dequeue();
                    var y = computer.Outputs.Dequeue();

                    if (destination == 255)
                    {
                        return new PuzzleAnswer(y, 22659);
                    }
                    
                    computers[destination].Inputs.Enqueue(x);
                    computers[destination].Inputs.Enqueue(y);
                }
            }
        }
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computers = new IntCodeComputer[50];

        // Init
        for (var computerIndex = 0; computerIndex < computers.Length; computerIndex++)
        {
            var computer = new IntCodeComputer();
            computer.Load(_program);
            computer.Inputs.Enqueue(computerIndex);

            computers[computerIndex] = computer;
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

                if (computer.Inputs.Count == 0)
                {
                    computer.Inputs.Enqueue(-1);
                }

                computer.Run();
                while (computer.Outputs.Count != 0)
                {
                    idle = false;

                    var destination = computer.Outputs.Dequeue();
                    var x = computer.Outputs.Dequeue();
                    var y = computer.Outputs.Dequeue();

                    if (destination == 255)
                    {
                        natX = x;
                        natY = y;
                    }
                    else
                    {
                        computers[destination].Inputs.Enqueue(x);
                        computers[destination].Inputs.Enqueue(y);
                    }
                }
            }

            if (idle)
            {
                if (lastNatY == natY)
                {
                    answer = natY;
                    break;
                }
                
                computers[0].Inputs.Enqueue(natX);
                computers[0].Inputs.Enqueue(natY);

                lastNatY = natY;                
            }
        }


        return new PuzzleAnswer(answer, 17429);
    }
}