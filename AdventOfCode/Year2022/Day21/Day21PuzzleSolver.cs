using AdventOfCode.Framework.Puzzle;
using System.Diagnostics;

namespace AdventOfCode.Year2022.Day21;

[Puzzle(2022, 21, "Monkey Math")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private OperationMonkey? RootMonkey;

    private NumberMonkey? HumanMonkey;

    public void ParseInput(string[] inputLines)
    {
        var knownMonkeys = new Dictionary<string, Monkey>();
        var leftDependantsOnMonkey = new Dictionary<string, OperationMonkey>();
        var rightDependantsOnMonkey = new Dictionary<string, OperationMonkey>();

        foreach (var line in inputLines)
        {
            var parseResult = MonkeyParse(line);
            knownMonkeys[parseResult.Monkey.Name] = parseResult.Monkey;

            // Deal with dependants
            if (leftDependantsOnMonkey.TryGetValue(parseResult.Monkey.Name, out var leftDependant))
            {
                leftDependant.SetLeftMonkey(parseResult.Monkey);
                leftDependantsOnMonkey.Remove(parseResult.Monkey.Name);

                parseResult.Monkey.SetParent(leftDependant);
            }

            if (rightDependantsOnMonkey.TryGetValue(parseResult.Monkey.Name, out var rightDependant))
            {
                rightDependant.SetRightMonkey(parseResult.Monkey);
                rightDependantsOnMonkey.Remove(parseResult.Monkey.Name);

                parseResult.Monkey.SetParent(rightDependant);
            }

            // Deal with current monkey
            if (parseResult.Monkey is NumberMonkey numberMonkey)
            {
                if (numberMonkey.Name == "humn")
                {
                    HumanMonkey = numberMonkey;
                }
            }
            else if (parseResult.Monkey is OperationMonkey operationMonkey)
            {
                if (parseResult.LeftMonkeyName == null || parseResult.RightMonkeyName == null)
                {
                    throw new InvalidOperationException();
                }

                // Left monkey
                if (knownMonkeys.TryGetValue(parseResult.LeftMonkeyName, out var leftMonkey))
                {
                    operationMonkey.SetLeftMonkey(leftMonkey);
                    leftMonkey.SetParent(operationMonkey);
                }
                else
                {
                    leftDependantsOnMonkey.Add(parseResult.LeftMonkeyName, operationMonkey);
                }

                // Right monkey
                if (knownMonkeys.TryGetValue(parseResult.RightMonkeyName, out var rightMonkey))
                {
                    operationMonkey.SetRightMonkey(rightMonkey);
                    rightMonkey.SetParent(operationMonkey);
                }
                else
                {
                    rightDependantsOnMonkey.Add(parseResult.RightMonkeyName, operationMonkey);
                }

                if (operationMonkey.Name == "root")
                {
                    RootMonkey = operationMonkey;
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        if (RootMonkey == null)
        {
            throw new InvalidOperationException("Root monkey is null");
        }

        var answer = RootMonkey.YellNumber();

        return new PuzzleAnswer(answer, 41857219607906L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Get the route from 'root' to 'humn'
        if (HumanMonkey == null)
        {
            throw new InvalidOperationException("Human monkey is null");
        }

        var monkeys = HumanMonkey.GetRouteFromRoot();

        //      r
        //     / \
        //   m1   m2
        //
        // r = result, m1 = monkey 1, m2 = monkey 2
        //
        // r = m1 - m2 = 0 => m1 = m2
        // r = m1 + m2 => m1 = r - m2 => m2 = r - m1
        // r = m1 - m2 => m1 = r + m2 => m2 = m1 - root
        // r = m1 * m2 => m1 = r / m2 => m2 = r / m1
        // r = m1 / m2 => m1 = r * m2 => m2 = m1 / root

        // Navigate from 'root' to 'humn' calculating the opposide side of the tree
        // and using reverse operations

        var result = 0L;
        for (var monkeyIndex = 0; monkeyIndex < monkeys.Count - 1; monkeyIndex++)
        {
            var monkey = monkeys[monkeyIndex];
            var nextMonkey = monkeys[monkeyIndex + 1];

            if (monkey is OperationMonkey operationMonkey && operationMonkey.LeftMonkey != null && operationMonkey.RightMonkey != null)
            {
                if (operationMonkey == RootMonkey)
                {
                    // Calculate the value of opposite side of the tree
                    var calculateMonkey = nextMonkey == operationMonkey.LeftMonkey ? operationMonkey.RightMonkey : operationMonkey.LeftMonkey;
                    result = calculateMonkey.YellNumber();
                }
                else if (nextMonkey == operationMonkey.LeftMonkey)
                {
                    // Human is on the left side of tree, calculate the value of right side of the tree
                    var monkeyValue = operationMonkey.RightMonkey.YellNumber();

                    result = operationMonkey.Operation switch
                    {
                        Operation.Add => result - monkeyValue,
                        Operation.Subtract => result + monkeyValue,
                        Operation.Multiply => result / monkeyValue,
                        Operation.Divide => monkeyValue * result,
                        _ => throw new NotImplementedException()
                    };
                }
                else if (nextMonkey == operationMonkey.RightMonkey)
                {
                    // Human is on the right side of tree, calculate the value of left side of the tree
                    var monkeyValue = operationMonkey.LeftMonkey.YellNumber();

                    result = operationMonkey.Operation switch
                    {
                        Operation.Add => result - monkeyValue,
                        Operation.Subtract => monkeyValue - result,
                        Operation.Multiply => result / monkeyValue,
                        Operation.Divide => monkeyValue / result,
                        _ => throw new NotImplementedException()
                    };
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        var answer = result;

        return new PuzzleAnswer(answer, 3916936880448L);
    }

    private static MonkeyParseResult MonkeyParse(string line)
    {
        var lineParts = line.Split(':');

        var monkeyName = lineParts[0];

        if (long.TryParse(lineParts[1], out var value))
        {
            return new MonkeyParseResult(new NumberMonkey(monkeyName, value), null, null);
        }
        else
        {
            var operationParts = lineParts[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var monkey1Name = operationParts[0];

            var operation = operationParts[1] switch
            {
                "+" => Operation.Add,
                "-" => Operation.Subtract,
                "*" => Operation.Multiply,
                "/" => Operation.Divide,
                _ => throw new UnreachableException()
            };

            var monkey2Name = operationParts[2];

            return new MonkeyParseResult(
                new OperationMonkey(monkeyName, operation),
                monkey1Name, monkey2Name);
        }
    }

    private sealed record MonkeyParseResult(Monkey Monkey, string? LeftMonkeyName, string? RightMonkeyName);
}