using AdventOfCode.Framework.Puzzle;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day07;

[Puzzle(2018, 7, "The Sum of Its Parts")]
public partial class Day07PuzzleSolver : IPuzzleSolver
{
    private readonly List<int> _namedSteps = [];
    private readonly List<List<int>> _stepDependencies = [];

    public void ParseInput(string[] inputLines)
    {
        for (var step = 'A'; step <= 'Z'; step++)
        {
            _stepDependencies.Add([]);
        }

        var namedSteps = new HashSet<int>();

        foreach (var line in inputLines)
        {
            var matches = InputLineRegex().Matches(line);
            if (matches.Count != 1)
            {
                throw new InvalidOperationException("Failed to parse input line");
            }

            var match = matches[0];

            var finishedBeforeStep = match.Groups[1].Value[0] - 'A';
            var step = match.Groups[2].Value[0] - 'A';

            namedSteps.Add(finishedBeforeStep);
            namedSteps.Add(step);

            _stepDependencies[step].Add(finishedBeforeStep);
        }

        _namedSteps.AddRange(namedSteps.OrderBy(x => x));
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = new StringBuilder();
        var workSteps = _namedSteps.ToList();

        while (workSteps.Count != 0)
        {
            var step = workSteps.Find(s => _stepDependencies[s].TrueForAll(x => !workSteps.Contains(x)));
            workSteps.Remove(step);

            answer.Append(Convert.ToChar(step + 'A'));
        }

        return new PuzzleAnswer(answer.ToString(), "SCLPAMQVUWNHODRTGYKBJEFXZI");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const int WORKERS_COUNT = 5;
        const int BASE_MINUTES = 60;

        var workers = Enumerable.Range(0, WORKERS_COUNT)
                                .Select(x => new Worker())
                                .ToList();

        var workSteps = _namedSteps.ToList();

        var minute = 0;

        while (true)
        {
            foreach (var worker in workers.Where(w => w.IsWorking))
            {
                worker.MinutesLeft--;

                if (worker.MinutesLeft == 0)
                {
                    worker.IsWorking = false;

                    workSteps.Remove(worker.ProcessingStep);
                }
            }

            if (workSteps.Count == 0)
            {
                break;
            }

            while (true)
            {
                var worker = workers.Find(w => !w.IsWorking);
                if (worker == null)
                {
                    break;
                }

                var step = workSteps.Cast<int?>()
                                    .FirstOrDefault(s => s != null &&
                                                         !workers.Exists(w => w.IsWorking && w.ProcessingStep == s) &&
                                                         _stepDependencies[s.Value].TrueForAll(x => !workSteps.Contains(x)));
                if (step == null)
                {
                    break;
                }

                worker.IsWorking = true;
                worker.ProcessingStep = step.Value;
                worker.MinutesLeft = BASE_MINUTES + step.Value + 1;
            }

            minute++;
        }

        return new PuzzleAnswer(minute, 1234);
    }

    [GeneratedRegex(@"Step ([A-Z]) must be finished before step ([A-Z]) can begin.")]
    private static partial Regex InputLineRegex();
}