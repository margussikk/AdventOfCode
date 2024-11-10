using AdventOfCode.Framework.Puzzle;
using System.Diagnostics;

namespace AdventOfCode.Year2022.Day21;

[Puzzle(2022, 21, "Monkey Math")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private OperationMonkey? _rootMonkey;

    private NumberMonkey? _humanMonkey;

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

            switch (parseResult.Monkey)
            {
                // Deal with the current monkey
                case NumberMonkey numberMonkey:
                {
                    if (numberMonkey.Name == "humn")
                    {
                        _humanMonkey = numberMonkey;
                    }

                    break;
                }
                case OperationMonkey when parseResult.LeftMonkeyName == null || parseResult.RightMonkeyName == null:
                    throw new InvalidOperationException();
                case OperationMonkey operationMonkey:
                {
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
                        _rootMonkey = operationMonkey;
                    }

                    break;
                }
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        if (_rootMonkey == null)
        {
            throw new InvalidOperationException("Root monkey is null");
        }

        var answer = _rootMonkey.YellNumber();

        return new PuzzleAnswer(answer, 41857219607906L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Get the route from 'root' to 'humn'
        if (_humanMonkey == null)
        {
            throw new InvalidOperationException("Human monkey is null");
        }

        var monkeys = _humanMonkey.GetRouteFromRoot();

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

        // Navigate from 'root' to 'humn' calculating the opposite side of the tree
        // and using reverse operations

        var result = 0L;
        for (var monkeyIndex = 0; monkeyIndex < monkeys.Count - 1; monkeyIndex++)
        {
            var monkey = monkeys[monkeyIndex];
            var nextMonkey = monkeys[monkeyIndex + 1];

            if (monkey is OperationMonkey { LeftMonkey: not null, RightMonkey: not null } operationMonkey)
            {
                if (operationMonkey == _rootMonkey)
                {
                    // Calculate the opposite side's value of the tree
                    var calculateMonkey = nextMonkey == operationMonkey.LeftMonkey ? operationMonkey.RightMonkey : operationMonkey.LeftMonkey;
                    result = calculateMonkey.YellNumber();
                }
                else if (nextMonkey == operationMonkey.LeftMonkey)
                {
                    // Human is on the left side of the tree, calculate the right side's value of the tree
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
                    // Human is on the right side of the tree, calculate the left side's value of the tree
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

    private sealed record MonkeyParseResult(Monkey Monkey, string? LeftMonkeyName, string? RightMonkeyName);
}