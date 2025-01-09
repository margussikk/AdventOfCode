using System.Numerics;

namespace AdventOfCode.Year2020.Day06;

internal class AnswerGroup
{
    public List<int> Answers { get; } = [];

    public int CountAnyoneAnswered()
    {
        var answerTotals = Answers.Aggregate(0, (current, answer) => current | answer);

        return BitOperations.PopCount((uint)answerTotals);
    }

    public int CountEveryoneAnswered()
    {
        var answerTotals = Answers.Aggregate(int.MaxValue, (current, answer) => current & answer);

        return BitOperations.PopCount((uint)answerTotals);
    }

    public static AnswerGroup Parse(string[] lines)
    {
        var answerGroup = new AnswerGroup();

        foreach (var line in lines)
        {
            var answer = line.Select(character => 1 << (character - 'a'))
                             .Aggregate(0, (current, bitmask) => current | bitmask);

            answerGroup.Answers.Add(answer);
        }

        return answerGroup;
    }
}
