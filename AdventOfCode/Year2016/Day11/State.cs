using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2016.Day11;
internal class State
{
    public int Elevator { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public int Steps { get; set; }

    public State Clone()
    {
        return new State
        {
            Elevator = Elevator,
            Items = [.. Items.Select(i => new Item { Element = i.Element, Type = i.Type, Floor = i.Floor })],
            Steps = Steps
        };
    }

    public State Copy(int elevator, int steps)
    {
        return new State
        {
            Elevator = elevator,
            Items = [.. Items.Select(i => new Item { Element = i.Element, Type = i.Type, Floor = i.Floor })],
            Steps = steps
        };
    }

    public bool IsValid()
    {
        // Assume items are sorted: generator, microchip, generator, microchip, ...
        for (var microshipIndex = 1; microshipIndex < Items.Count; microshipIndex += 2)
        {
            var microchip = Items[microshipIndex];
            var generator = Items[microshipIndex - 1];

            if (microchip.Floor == generator.Floor) continue; // Microchip is safe

            for (var otherGeneratorIndex = 0; otherGeneratorIndex < Items.Count; otherGeneratorIndex += 2)
            {
                var otherGenerator = Items[otherGeneratorIndex];
                if (otherGenerator.Floor == microchip.Floor)
                {
                    return false; // Microchip is fried
                }
            }
        }

        return true;
    }

    public void SortItems()
    {
        Items = [.. Items.OrderBy(i => i.Element).ThenBy(i => i.Type)];
    }

    public List<State> BuildNextStates()
    {
        var nextStates = new List<State>();
        var directions = new[] { -1, 1 }; // Down, Up

        foreach (var direction in directions)
        {
            var newFloor = Elevator + direction;
            if (newFloor < 1 || newFloor > 4) continue;

            var currentFloorItemIndexes = Items.Select((item, index) => (item, index))
                                               .Where(x => x.item.Floor == Elevator)
                                               .Select(x => x.index)
                                               .ToList();

            // Move one item
            foreach (var itemIndex in currentFloorItemIndexes)
            {
                var newState = Copy(newFloor, Steps + 1);
                newState.Items[itemIndex].Floor = newFloor;
                if (newState.IsValid())
                {
                    nextStates.Add(newState);
                }
            }

            // Move two items
            foreach (var indexPair in currentFloorItemIndexes.Pairs())
            {
                var newState = Copy(newFloor, Steps + 1);
                newState.Items[indexPair.First].Floor = newFloor;
                newState.Items[indexPair.Second].Floor = newFloor;
                if (newState.IsValid())
                {
                    nextStates.Add(newState);
                }
            }
        }

        return nextStates;
    }

    public int GetStateHash()
    {
        var elementItems = Items.Chunk(2)
                                .OrderBy(chunk => chunk[0].Floor)
                                .ThenBy(chunk => chunk[1].Floor);

        var state = Elevator;
        foreach (var item in elementItems)
        {
            state = state * 4 + item[0].Floor;
            state = state * 4 + item[1].Floor;
        }

        return state;
    }

    public static State Parse(string[] lines)
    {
        var items = new List<Item>();

        foreach (var line in lines)
        {
            var parts = line.Split(' ');

            var floor = parts[1] switch
            {
                "first" => 1,
                "second" => 2,
                "third" => 3,
                "fourth" => 4,
                _ => throw new InvalidOperationException($"Invalid floor: {parts[1]}")
            };

            for (var i = 4; i < parts.Length; i++)
            {
                var part = parts[i].TrimEnd(',', '.');

                var type = part switch
                {
                    "generator" => ItemType.Generator,
                    "microchip" => ItemType.Microchip,
                    _ => (ItemType?)null
                };

                if (type == null)
                {
                    continue;
                }

                items.Add(new Item
                {
                    Element = parts[i - 1].Split('-')[0],
                    Type = type.Value,
                    Floor = floor
                });
            }
        }

        return new State
        {
            Items = items,
            Elevator = 1,
            Steps = 0
        };
    }
}
