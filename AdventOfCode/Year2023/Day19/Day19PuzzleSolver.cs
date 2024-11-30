using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2023.Day19;

[Puzzle(2023, 19, "Aplenty")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private Dictionary<string, Workflow> _workflows = [];
    private List<Part> _parts = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _workflows = chunks[0]
            .Select(Workflow.Parse)
            .ToDictionary(w => w.Name);

        _parts = chunks[1]
            .Select(Part.Parse)
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        foreach (var part in _parts)
        {
            var workflowName = WorkflowName.In;
            while (workflowName != WorkflowName.Accepted && workflowName != WorkflowName.Rejected)
            {
                workflowName = _workflows[workflowName].GetNextWorkflowName(part);
            }

            if (workflowName == WorkflowName.Accepted)
            {
                answer += part.Ratings.Sum();
            }
        }

        return new PuzzleAnswer(answer, 432434);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0L;

        var queue = new Queue<WorkItem>();

        var currentWorkItem = new WorkItem
        {
            WorkflowName = WorkflowName.In,
            RatingNumberRanges =
            [
                new NumberRange<long>(1, 4000), // X
                new NumberRange<long>(1, 4000), // M
                new NumberRange<long>(1, 4000), // A
                new NumberRange<long>(1, 4000)  // S
            ]
        };

        queue.Enqueue(currentWorkItem);
        while (queue.TryDequeue(out currentWorkItem))
        {
            var workFlow = _workflows[currentWorkItem.WorkflowName];
            foreach (var rule in workFlow.Rules)
            {
                var nextWorkItem = new WorkItem
                {
                    WorkflowName = rule.WorkflowName,
                    RatingNumberRanges = [.. currentWorkItem.RatingNumberRanges]
                };

                switch (rule)
                {
                    case LessThanRule lessThanRule:
                        {
                            var ranges = currentWorkItem.RatingNumberRanges[lessThanRule.Rating].SplitBefore(lessThanRule.Number);

                            nextWorkItem.RatingNumberRanges[lessThanRule.Rating] = ranges[0];
                            currentWorkItem.RatingNumberRanges[lessThanRule.Rating] = ranges[1];
                            break;
                        }
                    case GreaterThanRule greaterThanRule:
                        {
                            var ranges = currentWorkItem.RatingNumberRanges[greaterThanRule.Rating].SplitAfter(greaterThanRule.Number);

                            currentWorkItem.RatingNumberRanges[greaterThanRule.Rating] = ranges[0];
                            nextWorkItem.RatingNumberRanges[greaterThanRule.Rating] = ranges[1];
                            break;
                        }
                    case NoConditionRule:
                        // Do nothing
                        break;
                }

                if (nextWorkItem.WorkflowName == WorkflowName.Accepted)
                {
                    answer += nextWorkItem.RatingNumberRanges[Rating.X].Length *
                              nextWorkItem.RatingNumberRanges[Rating.M].Length *
                              nextWorkItem.RatingNumberRanges[Rating.A].Length *
                              nextWorkItem.RatingNumberRanges[Rating.S].Length;
                }
                else if (nextWorkItem.WorkflowName != WorkflowName.Rejected)
                {
                    queue.Enqueue(nextWorkItem);
                }
            }
        }

        return new PuzzleAnswer(answer, 132557544578569L);
    }
}
