using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using System.Text;

namespace AdventOfCode.Year2024.Day24;

[Puzzle(2024, 24, "Crossed Wires")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<string, Wire> _wires = [];
    private readonly Dictionary<string, bool> _initialWireValues = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        // Initial wire values
        foreach (var line in chunks[0])
        {
            var splits = line.Split(':', StringSplitOptions.TrimEntries);

            var wire = new Wire(splits[0]);

            _wires[wire.Name] = wire;
            _initialWireValues[wire.Name] = splits[1][0] == '1';
        }

        // Gates
        foreach (var line in chunks[1])
        {
            var splits = line.Split([" ", "->"], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            // Wires
            var input1Wire = GetOrCreateWire(splits[0]);
            var input2Wire = GetOrCreateWire(splits[2]);
            var outputWire = GetOrCreateWire(splits[3]);

            // Gates
            var gateType = splits[1] switch
            {
                "AND" => GateType.And,
                "OR" => GateType.Or,
                "XOR" => GateType.Xor,
                _ => throw new NotImplementedException()
            };
            var gate = new Gate(gateType, outputWire);

            input1Wire.OutputGates.Add(gate);
            input2Wire.OutputGates.Add(gate);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        foreach (var valueKvp in _initialWireValues)
        {
            _wires[valueKvp.Key].SendValue(valueKvp.Value);
        }

        var wireString = _wires.Where(x => x.Key[0] == 'z')
                               .OrderByDescending(x => x.Value.Name, StringComparer.InvariantCulture)
                               .Select(x => x.Value.Value == true ? "1" : "0")
                               .Aggregate(new StringBuilder(), (agg, value) => agg.Append(value))
                               .ToString();

        var answer = Convert.ToInt64(wireString, 2);

        return new PuzzleAnswer(answer, 63168299811048L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var inputBitCount = _wires.Count(w => w.Key[0] == 'x');

        var swappedWireNames = new List<string>();

        for (var bitNumber = 0; bitNumber < inputBitCount; bitNumber++)
        {
            var wireNamesList = bitNumber == 0
                ? FindSwappedWiresInHalfAdder()
                : FindSwappedWiresInFullAdder(bitNumber);

            if (wireNamesList.Count != 0)
            {
                swappedWireNames.AddRange(wireNamesList);
            }
        }

        var answer = string.Join(",", swappedWireNames.OrderBy(x => x, StringComparer.InvariantCulture));

        return new PuzzleAnswer(answer, "dwp,ffj,gjh,jdr,kfm,z08,z22,z31");
    }

    private List<string> FindSwappedWiresInHalfAdder()
    {
        var bitNumberWireName = $"x00"; // x00 and y00 should lead to the same place, so just one one of them

        var xorGate = _wires[bitNumberWireName].OutputGates.First(g => g.GateType == GateType.Xor);
        var andGate = _wires[bitNumberWireName].OutputGates.First(g => g.GateType == GateType.And);

        // There is only one possible swapped wires pair
        if (xorGate.OutputWire.Name != "z00")
        {
            return
            [
                xorGate.OutputWire.Name,
                andGate.OutputWire.Name
            ];
        }

        return [];
    }


    private List<string> FindSwappedWiresInFullAdder(int bitNumber)
    {
        var swappedWires = new HashSet<string>();

        var visitedWireNames = new HashSet<string>();
        var visitedGates = new HashSet<Gate>();

        var bitNumberWireName = $"x{bitNumber:00}"; // x00 and y00 should lead to the same place, so just one one of them

        var queue = new Queue<Wire>();
        queue.Enqueue(_wires[bitNumberWireName]);

        while (queue.TryDequeue(out var wire))
        {
            if (!visitedWireNames.Add(wire.Name))
            {
                continue;
            }

            visitedWireNames.Add(wire.Name);
            visitedGates.UnionWith(wire.OutputGates);

            foreach (var gate in wire.OutputGates.Where(x => x.GateType != GateType.Or)) // Or gate leaves the current adder
            {
                queue.Enqueue(gate.OutputWire);
            }
        }

        var xorGate1 = _wires[bitNumberWireName].OutputGates.First(g => g.GateType == GateType.Xor);
        var xorGate2 = visitedGates.First(g => g.GateType == GateType.Xor && g != xorGate1);

        var andGate1 = _wires[bitNumberWireName].OutputGates.First(g => g.GateType == GateType.And);
        var andGate2 = visitedGates.First(g => g.GateType == GateType.And && g != andGate1);

        var orGate = visitedGates.First(g => g.GateType == GateType.Or);

        // Wire from XOR 1 gate needs to go to XOR 2 gate
        if (!xorGate1.OutputWire.OutputGates.Exists(g => g == xorGate2))
        {
            swappedWires.Add(xorGate1.OutputWire.Name);
        }

        // Wire from XOR 1 gate needs to go to AND 2 gate
        if (!xorGate1.OutputWire.OutputGates.Exists(g => g == andGate2))
        {
            swappedWires.Add(xorGate1.OutputWire.Name);
        }

        // Is z.. wire at XOR 2 gate's output
        if (xorGate2.OutputWire.Name != $"z{bitNumber:00}")
        {
            swappedWires.Add(xorGate2.OutputWire.Name);
            swappedWires.Add($"z{bitNumber:00}");
        }

        // Wire from AND 1 gate needs to go to OR gate
        if (!andGate1.OutputWire.OutputGates.Exists(g => g == orGate))
        {
            swappedWires.Add(andGate1.OutputWire.Name);
        }

        // Wire from AND 2 gate needs to go to OR gate
        if (!andGate2.OutputWire.OutputGates.Exists(g => g == orGate))
        {
            swappedWires.Add(andGate2.OutputWire.Name);
        }

        // Wire from OR gate should not go anywhere in this adder
        if (orGate.OutputWire.OutputGates.Exists(visitedGates.Contains))
        {
            swappedWires.Add(orGate.OutputWire.Name);
        }

        return [.. swappedWires];
    }

    private Wire GetOrCreateWire(string name)
    {
        if (!_wires.TryGetValue(name, out var wire))
        {
            wire = new Wire(name);
            _wires[wire.Name] = wire;
        }

        return wire;
    }
}