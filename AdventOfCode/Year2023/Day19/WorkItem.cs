using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2023.Day19;

internal class WorkItem
{
    public string WorkflowName { get; init; } = string.Empty;

    public NumberRange<long>[] RatingNumberRanges { get; set; } = new NumberRange<long>[4];
}
