using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day18;

[Puzzle(2020, 18, "Operation Order")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<Expression> _expressions = [];

    public void ParseInput(string[] inputLines)
    {
        _expressions = inputLines.Select(Expression.Parse)
                                 .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        static bool precedence(OperatorType previousOperatorType, OperatorType currentOperatorType) => true;

        var answer = _expressions.Sum(e => e.Evaluate(precedence));

        return new PuzzleAnswer(answer, 11004703763391L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        static bool precedence(OperatorType previousOperatorType, OperatorType currentOperatorType) => previousOperatorType <= currentOperatorType;

        var answer = _expressions.Sum(e => e.Evaluate(precedence));

        return new PuzzleAnswer(answer, 290726428573651L);
    }
}