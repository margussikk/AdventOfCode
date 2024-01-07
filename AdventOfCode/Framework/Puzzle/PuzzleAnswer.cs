namespace AdventOfCode.Framework.Puzzle;

public class PuzzleAnswer
{
    public string Answer { get; }
    public string ExpectedAnswer { get; }

    public PuzzleAnswer(string answer, string expectedAnswer)
    {
        Answer = answer;
        ExpectedAnswer = expectedAnswer;
    }

    public PuzzleAnswer(long answer, long expectedAnswer)
    {
        Answer = answer.ToString();
        ExpectedAnswer = expectedAnswer.ToString();
    }
}
