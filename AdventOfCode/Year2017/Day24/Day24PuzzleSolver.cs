using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day24;

[Puzzle(2017, 24, "Electromagnetic Moat")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<int, List<Component>> _componentsByPort = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var component in inputLines.Select(Component.Parse))
        {
            _componentsByPort.AddToValueList(component.Port1, component);

            if (component.Port1 != component.Port2)
            {
                _componentsByPort.AddToValueList(component.Port2, component);
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var cache = new Dictionary<(int, ulong), BridgeInfo>();

        var bridgeInfo = GetStrongestBridgeInfo(false, 0, 0, 0, 0UL, cache);

        return new PuzzleAnswer(bridgeInfo.Strength, 1859);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cache = new Dictionary<(int, ulong), BridgeInfo>();

        var bridgeInfo = GetStrongestBridgeInfo(true, 0, 0, 0, 0UL, cache);

        return new PuzzleAnswer(bridgeInfo.Strength, 1799);
    }

    private BridgeInfo GetStrongestBridgeInfo(bool longest, int port, int length, int strength, ulong usedBitmasks, Dictionary<(int, ulong), BridgeInfo> cache)
    {
        var key = (port, usedBitmasks);

        if (!cache.TryGetValue(key, out var bridgeInfo))
        {          
            bridgeInfo = new BridgeInfo(length, strength);

            var components = _componentsByPort.GetValueOrDefault(port, []);
            foreach (var candidate in components.Where(c => (usedBitmasks & c.Bitmask) == 0))
            {
                var nextPort = candidate.Port1 == port ? candidate.Port2 : candidate.Port1;
                var nextLength = length + 1;
                var nextStrength = strength + candidate.Strength;
                var nextUsedBitmasks = usedBitmasks | candidate.Bitmask;

                var tempBridgeInfo = GetStrongestBridgeInfo(longest, nextPort, nextLength, nextStrength, nextUsedBitmasks, cache);
                if ((!longest || tempBridgeInfo.Length >= bridgeInfo.Length) &&
                    tempBridgeInfo.Strength > bridgeInfo.Strength)
                {
                    bridgeInfo = tempBridgeInfo;
                }
            }           

            cache[key] = bridgeInfo;
        }

        return bridgeInfo;
    }

    private sealed record BridgeInfo(int Length, int Strength);
}