using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day08;

[Puzzle(2017, 8, "I Heard You Like Registers")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        GetHighestValues(out var answer, out _);

        return new PuzzleAnswer(answer, 4416);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        GetHighestValues(out _, out var answer);

        return new PuzzleAnswer(answer, 5199);
    }

    private void GetHighestValues(out int finalHighestValue, out int totalHighestValue)
    {
        totalHighestValue = int.MinValue;
        var registers = new Dictionary<string, int>();

        foreach (var instruction in _instructions)
        {
            var conditionRegisterValue = registers.GetValueOrDefault(instruction.ConditionRegister);
            var conditionSuccess = instruction.Condition switch
            {
                Condition.Greater => conditionRegisterValue > instruction.ConditionValue,
                Condition.GreaterEqual => conditionRegisterValue >= instruction.ConditionValue,
                Condition.Less => conditionRegisterValue < instruction.ConditionValue,
                Condition.LessEqual => conditionRegisterValue <= instruction.ConditionValue,
                Condition.Equal => conditionRegisterValue == instruction.ConditionValue,
                Condition.NotEqual => conditionRegisterValue != instruction.ConditionValue,
                _ => throw new InvalidOperationException()
            };

            if (conditionSuccess)
            {
                var operationRegisterValue = registers.GetValueOrDefault(instruction.OperationRegister);
                var value = instruction.Operation switch
                {
                    Operation.Inc => operationRegisterValue + instruction.OperationValue,
                    Operation.Dec => operationRegisterValue - instruction.OperationValue,
                    _ => throw new InvalidOperationException()
                };

                totalHighestValue = int.Max(totalHighestValue, value);

                registers[instruction.OperationRegister] = value;
            }
        }

        finalHighestValue = registers.Values.Max();
    }
}