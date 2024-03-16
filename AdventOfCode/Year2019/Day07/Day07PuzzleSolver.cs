using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day07;

[Puzzle(2019, 7, "Amplification Circuit")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new ();

    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        List<int> phaseSettingsOptions = [0, 1, 2, 3, 4];

        var answer = 0L;

        foreach (var phaseSettingSequence in phaseSettingsOptions.GetPermutations())
        {
            var inputSignal = 0L;

            foreach (var phaseSetting in phaseSettingSequence)
            {
                var computer = new IntCodeComputer();

                computer.Load(_program);
                computer.Inputs.Enqueue(phaseSetting);
                computer.Inputs.Enqueue(inputSignal);

                computer.Run();

                inputSignal = computer.Outputs.Dequeue();
            }

            answer = long.Max(inputSignal, answer);
        }

        return new PuzzleAnswer(answer, 366376);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        List<long> phaseSettingsOptions = [5, 6, 7, 8, 9];

        var answer = 0L;

        foreach (var phaseSettingSequence in phaseSettingsOptions.GetPermutations())
        {
            var computers = new List<IntCodeComputer>();

            foreach (var phaseSetting in phaseSettingSequence)
            {
                var computer = new IntCodeComputer();
                computer.Load(_program);
                computer.Inputs.Enqueue(phaseSetting);

                computers.Add(computer);
            }

            computers[0].Inputs.Enqueue(0);

            var halted = false;
            while (!halted)
            {
                for (var computerIndex = 0; computerIndex < computers.Count; computerIndex++)
                {
                    var exitCode = computers[computerIndex].Run();
                    if (exitCode == IntCodeExitCode.Halted && computerIndex == computers.Count - 1)
                    {
                        halted = true;
                        break;
                    }

                    while(computers[computerIndex].Outputs.TryDequeue(out var output))
                    {
                        computers[(computerIndex + 1) % computers.Count].Inputs.Enqueue(output);
                    }                   
                }
            }

            answer = long.Max(computers[^1].Outputs.Dequeue(), answer);
        }

        return new PuzzleAnswer(answer, 21596786);
    }
}