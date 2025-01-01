using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2018.Day08;

[Puzzle(2018, 8, "Memory Maneuver")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private Node? _rootNode;

    public void ParseInput(string[] inputLines)
    {
        var span = inputLines[0].SplitToNumbers<int>(' ').AsSpan();

        _rootNode = new Node(ref span);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        if (_rootNode == null)
        {
            throw new InvalidOperationException("Root node is null");
        }

        var answer = _rootNode.SumOfMetadataEntries();

        return new PuzzleAnswer(answer, 47647);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        if (_rootNode == null)
        {
            throw new InvalidOperationException("Root node is null");
        }

        var answer = _rootNode.GetValue();

        return new PuzzleAnswer(answer, 23636);
    }
}