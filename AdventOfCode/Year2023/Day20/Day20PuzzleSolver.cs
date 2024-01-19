using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2023.Day20;

[Puzzle(2023, 20, "Pulse Propagation")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private Dictionary<string, Module> _modules = [];

    public void ParseInput(string[] inputLines)
    {
        _modules = inputLines.Select(Module.Parse)
                             .ToDictionary(x => x.Name);

        var conjunctionModules = _modules
            .Where(x => x.Value is ConjunctionModule)
            .Select(x => x.Value)
            .Cast<ConjunctionModule>()
            .ToList();

        foreach (var conjunctionModule in conjunctionModules)
        {
            var inputModules = _modules
                .Where(x => x.Value.DestinationModules.Contains(conjunctionModule.Name))
                .Select(x => x.Value.Name)
                .ToArray();

            conjunctionModule.InitInputPulses(inputModules);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var highPulses = 0L;
        var lowPulses = 0L;

        var signalQueue = new Queue<Signal>();

        for (var i = 0; i < 1000; i++)
        {
            signalQueue.Enqueue(new Signal(ModuleName.Button, ModuleName.Broadcaster, false));
            while (signalQueue.TryDequeue(out var signal))
            {
                if (signal.Pulse)
                {
                    highPulses++;
                }
                else
                {
                    lowPulses++;
                }

                if (_modules.TryGetValue(signal.DestinationModule, out var module))
                {
                    var newSignals = module.ProcessSignal(signal);
                    foreach (var newSignal in newSignals)
                    {
                        signalQueue.Enqueue(newSignal);
                    }
                }
            }
        }

        var answer = highPulses * lowPulses;

        return new PuzzleAnswer(answer, 731517480L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        foreach (var kvp in _modules)
        {
            kvp.Value.Reset();
        }

        // Only one conjunction module sends signals to 'rx'. This module has multiple inputs and all of those need to be high at the same time.
        var feederModule = (ConjunctionModule)_modules.Values.First(x => x.DestinationModules.Contains(ModuleName.Rx));

        var counters = new Dictionary<string, long>();

        var buttonPresses = 0;
        while (counters.Count < feederModule.InputPulses.Count)
        {
            buttonPresses++;

            var signalQueue = new Queue<Signal>();
            signalQueue.Enqueue(new Signal(ModuleName.Button, ModuleName.Broadcaster, false));
            while (signalQueue.TryDequeue(out var signal))
            {
                if (signal.DestinationModule == feederModule.Name && signal.Pulse && !counters.ContainsKey(signal.SourceModule))
                {
                    counters[signal.SourceModule] = buttonPresses;
                }

                if (_modules.TryGetValue(signal.DestinationModule, out var module))
                {
                    var newSignals = module.ProcessSignal(signal);
                    foreach (var newSignal in newSignals)
                    {
                        signalQueue.Enqueue(newSignal);
                    }
                }
            }
        }

        var answer = MathFunctions.LeastCommonMultiple(counters.Values);

        return new PuzzleAnswer(answer, 244178746156661L);
    }
}
