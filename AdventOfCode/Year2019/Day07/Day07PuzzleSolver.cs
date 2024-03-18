using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day07;

[Puzzle(2019, 7, "Amplification Circuit")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SelectToLongs(',');
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
                var computer = new IntCodeComputer(_program);

                var result = computer.Run([phaseSetting, inputSignal]);

                inputSignal = result.Outputs[^1];
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
            var computerInputs = new List<List<long>>();

            foreach (var phaseSetting in phaseSettingSequence)
            {
                var computer = new IntCodeComputer(_program);
                computer.Run(phaseSetting);

                computers.Add(computer);
                computerInputs.Add([]);
            }

            computerInputs[0].Add(0);

            var halted = false;
            var lastComputerOutput = long.MinValue;
            while (!halted)
            {
                for (var computerIndex = 0; computerIndex < computers.Count; computerIndex++)
                {
                    var result = computers[computerIndex].Run(computerInputs[computerIndex]);
                    computerInputs[computerIndex].Clear();

                    if (result.ExitCode == IntCodeExitCode.Halted && computerIndex == computers.Count - 1)
                    {
                        lastComputerOutput = result.Outputs[^1];
                        halted = true;
                        break;
                    }

                    var nextComputerIndex = (computerIndex + 1) % computers.Count;
                    computerInputs[nextComputerIndex].AddRange(result.Outputs);
                }
            }

            answer = long.Max(lastComputerOutput, answer);
        }

        return new PuzzleAnswer(answer, 21596786);
    }
}