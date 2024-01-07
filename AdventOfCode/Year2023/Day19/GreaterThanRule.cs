namespace AdventOfCode.Year2023.Day19;

internal class GreaterThanRule(int rating, int number, string workflowName) : Rule(workflowName)
{
    public int Rating { get; } = rating;

    public int Number { get; } = number;

    public override bool Matches(Part part)
    {
        return part.Ratings[Rating] > Number;
    }
}
