namespace AdventOfCode.Year2022.Day11;

internal class Monkey
{
    public int Id { get; private set; }

    public List<long> StartingItems { get; private init; } = [];

    private readonly Queue<long> _items = new();
    public IReadOnlyCollection<long> Items => _items;

    public Operation Operation { get; private init; } = new MultiplyOperation(new NumberOperand(0));

    public int TestDivisor { get; private init; }

    public int ThrowToMonkeyIfTrue { get; private init; }

    public int ThrowToMonkeyIfFalse { get; private init; }

    public int Inspections { get; private set; }

    public long TakeOutInspectedItem()
    {
        Inspections++;
        return _items.Dequeue();
    }

    public long CalculateWorryLevel(long worryLevel)
    {
        return Operation.CalculateWorryLevel(worryLevel);
    }

    public void CatchItem(long item)
    {
        _items.Enqueue(item);
    }

    public void Reset()
    {
        _items.Clear();

        foreach (var item in StartingItems)
        {
            _items.Enqueue(item);
        }

        Inspections = 0;
    }

    public int DecideCatcher(long worryLevel)
    {
        return worryLevel % TestDivisor == 0
            ? ThrowToMonkeyIfTrue
            : ThrowToMonkeyIfFalse;
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
        IOperand operand = operationSplits[2] == "old"
            ? new OldOperand()
            : new NumberOperand(int.Parse(operationSplits[2]));

        Operation operation = operationSplits[1] == "*"
            ? new MultiplyOperation(operand)
            : new AddOperation(operand);

        // Test: divisible by ...
        var testDivisor = int.Parse(lines[3].Split(' ')[^1]);

        // If true: throw to monkey ...
        var throwToMonkeyIfTestTrue = int.Parse(lines[4].Split(' ')[^1]);

        // If false: throw to monkey ...
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
