using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2024.Day24;

namespace AdventOfCode.Year2015.Day07;

[Puzzle(2015, 7, "Some Assembly Required")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Wire> _wires = [];

    private readonly List<BufferGate> _constantBufferGates = [];

    public void ParseInput(string[] inputLines)
    {
        var wires = new Dictionary<string, Wire>();

        foreach (var line in inputLines)
        {
            var parts = line.Split(' ');

            if (parts.Length == 3)
            {
                var gate = new BufferGate
                {
                    OutputWire = GetOrCreateWire(parts[2])
                };

                ConnectInputPort(parts[0], gate.Input);

                if (gate.Input.Signal.HasValue)
                {
                    _constantBufferGates.Add(gate);
                }
            }
            else if (parts.Length == 4)
            {
                var gate = new NotGate
                {
                    OutputWire = GetOrCreateWire(parts[3])
                };

                ConnectInputPort(parts[1], gate.Input);
            }
            else if (parts.Length == 5)
            {
                if (parts[1] == "AND")
                {
                    var gate = new AndGate
                    {
                        OutputWire = GetOrCreateWire(parts[4])
                    };

                    ConnectInputPort(parts[0], gate.Input1);
                    ConnectInputPort(parts[2], gate.Input2);
                }
                else if (parts[1] == "OR")
                {
                    var gate = new OrGate
                    {
                        OutputWire = GetOrCreateWire(parts[4])
                    };

                    ConnectInputPort(parts[0], gate.Input1);
                    ConnectInputPort(parts[2], gate.Input2);
                }
                else if (parts[1] == "LSHIFT")
                {
                    var gate = new LeftShiftGate
                    {
                        OutputWire = GetOrCreateWire(parts[4])
                    };

                    ConnectInputPort(parts[0], gate.Input1);
                    ConnectInputPort(parts[2], gate.Input2);
                }
                else if (parts[1] == "RSHIFT")
                {
                    var gate = new RightShiftGate
                    {
                        OutputWire = GetOrCreateWire(parts[4])
                    };

                    ConnectInputPort(parts[0], gate.Input1);
                    ConnectInputPort(parts[2], gate.Input2);
                }
                else
                {
                    throw new InvalidOperationException("Invalid input");
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid input");
            }
        }

        _wires = [.. wires.Values];

        void ConnectInputPort(string name, Port port)
        {
            if (int.TryParse(name, out var signal))
            {
                port.Signal = signal;
            }
            else
            {
                var wire = GetOrCreateWire(name);
                wire.Ports.Add(port);
            }
        }

        Wire GetOrCreateWire(string name)
        {
            if (!wires.TryGetValue(name, out var wire))
            {
                wire = new Wire(name);
                wires.Add(wire.Name, wire);
            }

            return wire;
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetASignal();

        return new PuzzleAnswer(answer, 46065);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var bAssignGate = _constantBufferGates.First(g => g.OutputWire.Name == "b");
        bAssignGate.Input.Signal = GetASignal();

        var answer = GetASignal();

        return new PuzzleAnswer(answer, 14134);
    }

    private int GetASignal()
    {
        foreach (var gate in _constantBufferGates)
        {
            gate.PerformLogic();
        }

        var aSignal = 0;

        foreach (var wire in _wires)
        {
            if (wire.Name == "a")
            {
                aSignal = wire.Signal!.Value;
            }

            wire.CarrySignal(null);
        }

        return aSignal;
    }
}