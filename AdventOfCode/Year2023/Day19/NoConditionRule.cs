namespace AdventOfCode.Year2023.Day19;

internal class NoConditionRule(string workflowName) : Rule(workflowName)
{
    public override bool Matches(Part part)
    {
        return true;
    }
}
