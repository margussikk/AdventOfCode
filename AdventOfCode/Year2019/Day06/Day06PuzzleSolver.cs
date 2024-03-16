using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2019.Day06;

[Puzzle(2019, 6, "Universal Orbit Map")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<string, SpaceObject> _spaceObjects = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var splits = line.Split(')');

            if (!_spaceObjects.TryGetValue(splits[0], out var parentObject))
            {
                parentObject = new SpaceObject(splits[0]);
                _spaceObjects.Add(parentObject.Name, parentObject);
            }

            if (!_spaceObjects.TryGetValue(splits[1], out var childObject))
            {
                childObject = new SpaceObject(splits[1]);
                _spaceObjects.Add(childObject.Name, childObject);
            }

            childObject.Orbits = parentObject;
            parentObject.OrbitedBy.Add(childObject);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _spaceObjects["COM"].CountOrbits(0);

        return new PuzzleAnswer(answer, 453028);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var steps = 0;
        var transfers = new Dictionary<string, int>();

        var spaceObject = _spaceObjects["YOU"].Orbits;
        while (spaceObject != null)
        {
            transfers[spaceObject.Name] = steps++;
            spaceObject = spaceObject.Orbits;
        }

        steps = 0;
        spaceObject = _spaceObjects["SAN"].Orbits;
        while (!transfers.ContainsKey(spaceObject!.Name))
        {
            steps++;
            spaceObject = spaceObject.Orbits;
        }

        var answer = steps + transfers[spaceObject.Name];

        return new PuzzleAnswer(answer, 562);
    }
}