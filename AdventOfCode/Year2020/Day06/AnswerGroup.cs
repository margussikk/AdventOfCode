using System.Numerics;

namespace AdventOfCode.Year2020.Day06;

internal class AnswerGroup
{
    public List<int> Answers { get; private set; } = [];

    public int CountAnyoneAnswered()
    {
        var answerTotals = 0;

        foreach (var answer in Answers)
        {
            answerTotals |= answer;
        }

        return BitOperations.PopCount((uint)answerTotals);
    }

    public int CountEveryoneAnswered()
    {
        var answerTotals = int.MaxValue;

        foreach (var answer in Answers)
        {
            answerTotals &= answer;
        }

        return BitOperations.PopCount((uint)answerTotals);
    }

    public static AnswerGroup Parse(string[] lines)
    {
        var answerGroup = new AnswerGroup();

        foreach (var line in lines)
        {
            var answer = 0;

            foreach (var character in line)
            {
                var bitmask = 1 << (character - 'a');
                answer |= bitmask;
            }

            answerGroup.Answers.Add(answer);
        }

        return answerGroup;
    }
}
