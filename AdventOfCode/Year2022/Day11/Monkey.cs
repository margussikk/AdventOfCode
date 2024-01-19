namespace AdventOfCode.Year2022.Day11;

internal class Monkey
{
    public int Id { get; private set; }

    public List<long> StartingItems { get; private set; } = [];

    private readonly Queue<long> items = new();
    public IReadOnlyCollection<long> Items => items;

    public Operation Operation { get; private set; } = new MultiplyOperation(new NumberOperand(0));

    public int TestDivisor { get; private set; }

    public int ThrowToMonkeyIfTrue { get; private set; }

    public int ThrowToMonkeyIfFalse { get; private set; }

    public int Inspections { get; private set; }

    public long TakeOutInspectedItem()
    {
        Inspections++;
        return items.Dequeue();
    }

    public long CalculateWorryLevel(long worryLevel)
    {
        return Operation.CalculateWorryLevel(worryLevel);
    }

    public void CatchItem(long item)
    {
        items.Enqueue(item);
    }

    public void Reset()
    {
        items.Clear();

        foreach (var item in StartingItems)
        {
            items.Enqueue(item);
        }

        Inspections = 0;
    }

    public int DecideCatcher(long worryLevel)
    {
        if (worryLevel % TestDivisor == 0)
        {
            return ThrowToMonkeyIfTrue;
        }

        return ThrowToMonkeyIfFalse;
    }

    public static Monkey Parse(string[] lines)
    {
        // Id
        var monkeyId = int.Parse(lines[0]["Monkey ".Length..^1]);

        // StartingItems
        var startingItems = lines[1]
            .Split(": ")
            [^1]
            .Split(',')
            .Select(long.Parse)
            .ToList();

        // Operation
        var operationSplits = lines[2]
            .Split(" = ")
            [^1]
            .Split(' ');

        // operationSplits[0] is always 'old' so ignore that
        Operand operand = operationSplits[2] == "old"
            ? new OldOperand()
            : new NumberOperand(int.Parse(operationSplits[2]));

        Operation operation = operationSplits[1] == "*"
            ? new MultiplyOperation(operand)
            : new AddOperation(operand);

        // Test: divisible by ..
        var testDivisor = int.Parse(lines[3].Split(' ')[^1]);

        // If true: throw to monkey ..
        var throwToMonkeyIfTestTrue = int.Parse(lines[4].Split(' ')[^1]);

        // If false: throw to monkey ..
        var throwToMonkeyIfTestFalse = int.Parse(lines[5].Split(' ')[^1]);

        return new Monkey
        {
            Id = monkeyId,
            StartingItems = startingItems,
            Operation = operation,
            TestDivisor = testDivisor,
            ThrowToMonkeyIfTrue = throwToMonkeyIfTestTrue,
            ThrowToMonkeyIfFalse = throwToMonkeyIfTestFalse
        };
    }
}
