namespace AdventOfCode.Year2023.Day19;

internal class LessThanRule : Rule
{
    public int Rating { get; }

    public int Number { get; }

    public LessThanRule(int rating, int number, string workflowName) : base(workflowName)
    {
        Rating = rating;
        Number = number;
    }

    public override bool Matches(Part part)
    {
        return part.Ratings[Rating] < Number;
    }
}
