namespace AdventOfCode.Year2023.Day19;

internal class Workflow
{
    public string Name { get; private init; } = string.Empty;
    public List<Rule> Rules { get; private init; } = [];

    public string GetNextWorkflowName(Part part)
    {
        return Rules.First(x => x.Matches(part)).WorkflowName;
    }

    public static Workflow Parse(string input)
    {
        var rulesStartIndex = input.IndexOf('{');

        var workFlow = new Workflow
        {
            Name = input[..rulesStartIndex],
            Rules = input[rulesStartIndex..]
                         .Replace("{", string.Empty)
                         .Replace("}", string.Empty)
                         .Split(',')
                         .Select(Rule.Parse)
                         .ToList()
        };

        return workFlow;
    }
}
